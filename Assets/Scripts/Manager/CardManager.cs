using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public TurnManager turnManager;
    public DiscardPile discardPile;

    void Start()
    {
        foreach (Player player in turnManager.players)
        {
            player.handStable.cardManager = this;
        }
    }

    void Update()
    {
        
    }

    public void DrawCard(Card card, CardSpace currentCardSpace, Player player)
    {
        card.RevealCard();
        MoveCard(card, currentCardSpace, player.handStable);
    }

    public bool PlayCardForCurrentPlayer(Card card, HandStable handStable)
    {
        Player opponent = turnManager.players.FirstOrDefault(p => p != turnManager.activePlayer);
        if (!card.cardData.CanPlay(turnManager.activePlayer, opponent))
        {
            Debug.LogWarning($"Cannot play {card.name}: play conditions not met.");
            return false;
        }

        // Give the other player a window to Neigh this play. If a contest begins, the card stays
        // in hand and NeighManager drives resolution (and the turn-phase advance) once it settles;
        // the caller treats this as a successful play. TryBeginContest returns false for anything
        // that can't be Neigh'd right now (no Neigh in the responder's hand, mid-effect play, etc.),
        // in which case we resolve immediately as before.
        if (NeighManager.Instance != null && NeighManager.Instance.TryBeginContest(card, handStable))
        {
            return true;
        }

        return ResolvePlay(card, handStable);
    }

    // Everything that happens once a play is confirmed (not Neigh'd): fire an IMMEDIATE effect,
    // then route the card to its destination. Split out of PlayCardForCurrentPlayer so NeighManager
    // can call it after a contest resolves in the caster's favour.
    public bool ResolvePlay(Card card, HandStable handStable)
    {
        Player opponent = turnManager.players.FirstOrDefault(p => p != turnManager.activePlayer);

        // Cards that stay in play (Unicorn/Upgrade/Downgrade) enter their stable BEFORE their
        // IMMEDIATE effect fires — "when this card enters your Stable" — so the effect sees the
        // card in play (e.g. Cult Leader Unicorn can be sacrificed to its own effect). Magic/Neigh
        // cards fire first and are discarded after, so they're still in hand while the effect runs.
        switch (card.cardData.cardType)
        {
            case CardType.UNICORN:
                MoveCard(card, handStable, turnManager.activePlayer.unicornStable);
                turnManager.activePlayer.unicornStable.CheckWinCondition();
                TriggerIfImmediate(card);
                return true;
            case CardType.UPGRADE:
                MoveCard(card, handStable, turnManager.activePlayer.upgradeStable);
                TriggerIfImmediate(card);
                return true;
            case CardType.DOWNGRADE:
                if (opponent == null)
                {
                    Debug.Log($"Player {turnManager.activePlayer.name} has no opponent");
                    return false;
                }

                MoveCard(card, handStable, opponent.downgradeStable);
                TriggerIfImmediate(card);
                return true;
            case CardType.MAGIC:
            case CardType.NEIGH:
                TriggerIfImmediate(card);
                MoveCard(card, handStable, discardPile);
                return true;
            default:
                return false;
        }
    }

    private void TriggerIfImmediate(Card card)
    {
        if (card.cardData.specialActionType == SpecialActionType.IMMEDIATE)
        {
            card.cardData.TriggerSpecialAction(card);
        }
    }

    public void MoveCard(Card card, CardSpace oldCardSpace, CardSpace newCardSpace)
    {
        // Baby Unicorns never go to the discard pile, or back to a hand from a stable — anything
        // that would put one there (sacrifice, destroy, discard, return-to-hand) sends it to the
        // Nursery instead. Nursery -> hand (the opening deal) and hand -> hand are unaffected.
        bool stableToHand = newCardSpace is HandStable && oldCardSpace is Stable && !(oldCardSpace is HandStable);
        if ((newCardSpace is DiscardPile || stableToHand)
            && card.cardData is UnicornCardData unicorn && unicorn.unicornType == UnicornType.BABY
            && DeckManager.Instance != null && DeckManager.Instance.nursery != null)
        {
            newCardSpace = DeckManager.Instance.nursery;
        }

        oldCardSpace.RemoveCard(card);
        newCardSpace.AddCard(card);

        // If this card is leaving a UnicornStable and has a linked Baby Unicorn (only ever set
        // for cards implementing IReturnsStolenCardOnLeave, e.g. Free Candy Unicorn), send that
        // Baby Unicorn back to its original stable — but only if it's still sitting where it was
        // left; if it was itself discarded/destroyed/moved elsewhere since, skip silently rather
        // than resurrecting it.
        if (oldCardSpace is UnicornStable && card.cardData is IReturnsStolenCardOnLeave && card.linkedBabyUnicorn != null)
        {
            Card babyUnicorn = card.linkedBabyUnicorn;
            CardSpace originStable = card.linkedBabyUnicornOriginStable;
            card.linkedBabyUnicorn = null;
            card.linkedBabyUnicornOriginStable = null;

            if (babyUnicorn.cardSpace == oldCardSpace)
            {
                MoveCard(babyUnicorn, babyUnicorn.cardSpace, originStable);
            }
        }

        CheckSelfSacrificeConditions();
    }

    private bool checkingSelfSacrifice;

    // "If at any time ..., SACRIFICE this card" (ISelfSacrificeCondition). Every board change goes
    // through MoveCard, so re-checking here after each move is "at any time". The sacrifice itself
    // is a MoveCard, so the guard stops re-entry; the loop re-scans in case one sacrifice makes
    // another card's condition true.
    private void CheckSelfSacrificeConditions()
    {
        if (checkingSelfSacrifice || turnManager == null || turnManager.players == null) return;
        checkingSelfSacrifice = true;
        try
        {
            bool sacrificedAny = true;
            while (sacrificedAny)
            {
                sacrificedAny = false;
                foreach (Player player in turnManager.players)
                {
                    foreach (Stable stable in new Stable[] { player.unicornStable, player.upgradeStable, player.downgradeStable })
                    {
                        Card doomed = stable.spaceCards.Find(c =>
                            c.cardData is ISelfSacrificeCondition condition && condition.ShouldSacrifice(player));
                        if (doomed == null) continue;

                        MoveCard(doomed, stable, discardPile);
                        stable.RepositionCards();
                        Debug.Log($"{doomed.name}'s condition was met — sacrificed from {player.name}'s Stable.");
                        sacrificedAny = true;
                    }
                }
            }
        }
        finally
        {
            checkingSelfSacrifice = false;
        }
    }
}
