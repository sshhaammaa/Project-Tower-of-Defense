using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMonety : MonoBehaviour
{

    public static PlayerMonety main;

    [SerializeField] 
    public int money = 0;


    [SerializeField] private TextMeshProUGUI MoneyGUI;
    void Awake()
    {
        main = this;
    }

    
    void Update()
    {
        MoneyGUI.text = "Money: " + money.ToString();
    }
}
