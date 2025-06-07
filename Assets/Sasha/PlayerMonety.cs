using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMonety : MonoBehaviour
{
    // Статична змінна для доступу до екземпляра класу 
    public static PlayerMonety instance;

    // Початкова кількість грошей 
    [SerializeField] private int money = 1000;

    // Посилання на текстовий елемент для відображення грошей
    [SerializeField] private TextMeshProUGUI MoneyGUI;

    // Метод Awake викликається при активації об'єкта
    void Awake()
    {
        // Забезпечує, що існує лише один екземпляр PlayerMonety
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject); // Якщо вже є, знищує дублікат
    }

    // Оновлення тексту на екрані щокадрово
    void Update()
    {
        MoneyGUI.text = "Money: " + money;
    }

    // Метод витрати грошей
    public bool SpendMoney(int amount)
    {
        // Якщо грошей достатньо — віднімаємо і повертаємо true
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        return false; // Інакше повертаємо false
    }

    // Метод додавання грошей
    public void AddMoney(int amount)
    {
        money += amount;
    }

    // Метод для отримання поточної кількості грошей
    public int GetMoney()
    {
        return money;
    }
}
