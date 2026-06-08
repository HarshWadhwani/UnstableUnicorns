using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/HentaicornCardData")]
public class HentaicornCardData : UnicornCardData, ISacrificeShield
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.MAGIC;
    }

    // Intercepts any destroy that could target unicorns (Any or Unicorn scope).
    public bool CanInterceptDestroy(DestroyCardAction.TargetStable targetStable)
    {
        return true;
    }
}
