using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/BasicUnicornCardData")]
public class BasicUnicornCardData : UnicornCardData
{
    private int nextNameIndex;

    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.BASIC;
        nextNameIndex = 0;
    }

    public override string NextCardName()
    {
        string cardName = cardNameVariations[nextNameIndex % cardNameVariations.Count];
        nextNameIndex++;
        return cardName;
    }
}
