// Implemented by any CardData that can sacrifice itself to intercept a destroy action.
// DestroyCardAction checks for this before prompting the destroyer to pick a card.
public interface ISacrificeShield
{
    // Returns true if this card's ability should intercept the given destroy scope.
    bool CanInterceptDestroy(DestroyCardAction.TargetStable targetStable);
}
