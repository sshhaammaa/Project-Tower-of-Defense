using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMonety : MonoBehaviour
{
    public static PlayerMonety instance;

    [SerializeField] private int money = 100;
    [SerializeField] private TextMeshProUGUI MoneyGUI;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        MoneyGUI.text = "Money: " + money;
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        return false;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public int GetMoney()
    {
        return money;
    }
}