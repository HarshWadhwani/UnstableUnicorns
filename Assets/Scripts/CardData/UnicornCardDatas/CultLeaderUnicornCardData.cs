using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/CultLeaderUnicornCardData")]
public class CultLeaderUnicornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
        specialActionType = SpecialActionType.IMMEDIATE;

        // "When this card enters your Stable, each player (including you) must SACRIFICE a Unicorn
        // card." This card is already in the caster's Stable when the effect fires
        // (CardManager.ResolvePlay routes Unicorns first), so the caster can always sacrifice it.
        actions = new List<CardAction>
        {
            new ForEachPlayerAction
            {
                actions = new List<CardAction>
                {
                    new SacrificeCardAction
                    {
                        targetStable = SacrificeCardAction.TargetStable.Unicorn,
                        sacrificeAll = false,
                        numberOfCards = 1
                    }
                }
            }
        };
    }
}
