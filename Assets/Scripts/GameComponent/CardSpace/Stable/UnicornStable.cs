using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornStable : Stable
{
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void CheckWinCondition()
    {
        if (spaceCards.Count == maxCardsInStable)
        {
            Debug.Log("Player wins!");
        }
    }
}

