using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardData/UnicornCardData/BabyUnicornCardData")]
public class BabyUnicornCardData : UnicornCardData
{
    public override void TriggerSpecialAction()
    {
        Debug.Log("No Special Action for BabyUnicornCardData");
    }

    public override void OnEnable()
    {
        base.OnEnable();
        unicornType = UnicornType.BABY;
    }
}
