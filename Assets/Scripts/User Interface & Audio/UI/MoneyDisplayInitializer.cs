using UnityEngine;
using TMPro;

public class MoneyDisplayInitializer : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    void Start()
    {
        if (MoneySystem.Instance != null)
        {
            MoneySystem.Instance.SetMoneyText(moneyText);
        }
    }
}
