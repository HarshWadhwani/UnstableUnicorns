using UnityEngine;

[System.Serializable]
public class MoveSelfToOpponentStableAction : CardAction
{
    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        if (context.sourceCard == null)
        {
            Debug.LogWarning("MoveSelfToOpponentStableAction: no sourceCard in context.");
            return;
        }

        CardSpace source = context.activePlayer.unicornStable;
        CardSpace destination = context.opponentPlayer.unicornStable;

        if (!source.spaceCards.Contains(context.sourceCard))
        {
            Debug.LogWarning($"MoveSelfToOpponentStableAction: {context.sourceCard.name} is not in active player's unicorn stable.");
            return;
        }

        context.cardManager.MoveCard(context.sourceCard, source, destination);
        Debug.Log($"{context.sourceCard.name} moved to {context.opponentPlayer.name}'s stable.");
    }
}
