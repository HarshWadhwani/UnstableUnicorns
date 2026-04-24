using UnityEngine;

[System.Serializable]
public abstract class CardAction
{
    public abstract void Execute(CardActionExecutor executor, CardActionContext context);
}
