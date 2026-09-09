using System.Text;
using UnityEngine;

// Peeping Narwhal: "each other player must show you their hand." No cards move. Hands are
// otherwise face-down for non-active players (HandVisibilityController) — this reveals each
// other player's hand to the caster on the caster's next turn (casting a Unicorn ends the
// current turn immediately, so there's no useful window this turn). Also logs the hands.
[System.Serializable]
public class RevealHandsAction : CardAction
{
    public override void Execute(CardActionExecutor executor, CardActionContext context)
    {
        foreach (Player player in context.turnManager.players)
        {
            if (player == context.activePlayer) continue;

            HandVisibilityController.Instance?.RevealOnCasterNextTurn(context.activePlayer, player);

            var sb = new StringBuilder();
            foreach (Card card in player.handStable.spaceCards)
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append(card.name);
            }
            Debug.Log($"[Peeping Narwhal] {player.name}'s hand ({player.handStable.spaceCards.Count}): {sb} "
                      + $"— revealed to {context.activePlayer.name} next turn.");
        }
    }
}
