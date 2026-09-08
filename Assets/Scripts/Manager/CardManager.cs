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

        if (card.cardData.specialActionType == SpecialActionType.IMMEDIATE)
        {
            card.cardData.TriggerSpecialAction(card);
        }

        switch (card.cardData.cardType)
        {
            case CardType.UNICORN:
                MoveCard(card, handStable, turnManager.activePlayer.unicornStable);
                turnManager.activePlayer.unicornStable.CheckWinCondition();
                return true;
            case CardType.UPGRADE:
                MoveCard(card, handStable, turnManager.activePlayer.upgradeStable);
                return true;
            case CardType.DOWNGRADE:
                if (opponent == null)
                {
                    Debug.Log($"Player {turnManager.activePlayer.name} has no opponent");
                    return false;
                }

                MoveCard(card, handStable, opponent.downgradeStable);
                return true;
            case CardType.MAGIC:
            case CardType.NEIGH:
                MoveCard(card, handStable, discardPile);
                return true;
            default:
                return false;
        }
    }

    public void MoveCard(Card card, CardSpace oldCardSpace, CardSpace newCardSpace)
    {
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
    }
}
