using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/BasicUnicornCardData")]
public class BasicUnicornCardData : UnicornCardData
{
    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.BASIC;
    }
}
