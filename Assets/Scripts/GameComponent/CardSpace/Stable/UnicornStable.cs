using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicornStable : Stable
{
    public int winConditionCount = 7;

    void Start()
    {

    }

    void Update()
    {

    }

    public void CheckWinCondition()
    {
        if (spaceCards.Count >= winConditionCount)
        {
            Debug.Log($"{player.name} wins!");
        }
    }
}

