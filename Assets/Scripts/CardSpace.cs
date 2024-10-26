using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class CardSpace : MonoBehaviour
{
    public TurnManager turnManager;

    public abstract void HandleCardClick(Card card);
}
