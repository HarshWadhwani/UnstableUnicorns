using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BabyUnicornCardScriptableObject", menuName = "ScriptableObjects/UnicornCards/BabyUnicornCard")]
public class BabyUnicornCardScriptableOject : UnicornCardScriptableOject
{
    private void OnEnable()
    {
        cardNameVariations = new List<string>
        {
            "Shotgun Baby Unicorn",
            "Ugly Baby Unicorn"
        };
        cardDescriptionText = "";
        instances = 2;
        unicornType = UnicornType.Baby;
    }

    public override void TriggerSpecialAction()
    {
        Debug.Log("Triggered special action for BabyUnicornCardScriptableOject");
    }
}
