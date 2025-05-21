// Scripts/AddMoneyButton.cs
using UnityEngine;

public class AddMoneyButton : MonoBehaviour
{
    public int moneyToAdd = 100; 

    public void GiveMoney()
    {
        if (MoneySystem.Instance != null)
        {
            MoneySystem.Instance.AddMoney(moneyToAdd);
            Debug.Log($"Player earned R{moneyToAdd}");
        }
        else
        {
            Debug.LogError("MoneySystem.Instance is null!");
        }
    }
}
