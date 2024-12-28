using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornStable : Stable
{


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void AddCardToStable(Card card)
    {
        AddCardToSpace(card);
        PositionCardsInStable();
    }

    public void CheckWinCondition()
    {
        if (spaceCards.Count == maxCardsInStable)
        {
            Debug.Log("Player wins!");
        }
    }
}
