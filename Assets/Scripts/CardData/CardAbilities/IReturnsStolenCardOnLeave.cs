// Implemented by any CardData whose stolen Baby Unicorn should return to its original stable
// when this card itself leaves a UnicornStable. CardManager.MoveCard checks for this after
// moving a card out of a UnicornStable; the linkage is recorded on Card.linkedBabyUnicorn by
// CardActionExecutor.ExecutePendingAction when the steal resolves, and only ever for a Baby
// Unicorn — never for any other stolen card.
public interface IReturnsStolenCardOnLeave
{
}
