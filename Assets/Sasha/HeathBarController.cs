using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarManager : MonoBehaviour
{
    // Статичний екземпляр класу (Singleton)
    public static HealthBarManager instance;

    [Header("UI")]
    public Slider healthSlider;                 // Слайдер для відображення здоров'я
    public TextMeshProUGUI healthText;          // Текст поточного здоров'я
    public GameObject gameOverPanel;            // Панель Game Over
    public Image backgroundImage;               // Фон панелі
    public Image fillImage;                     // Заливка слайдера
    public Color normalColor = Color.green;     // Колір у нормальному стані
    public Color gameOverColor = Color.red;     // Колір при поразці

    [Header("Настройки бази")]
    public int maxHP = 100;         // Максимальне здоров'я
    private int currentHP;          // Поточне здоров'я

    private void Awake()
    {
        
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        currentHP = maxHP;

        // Ініціалізація слайдера здоров'я
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHP;
            healthSlider.minValue = 0;
        }

        UpdateUI(); // Оновити інтерфейс
    }


    // Метод викликається для завдання шкоди базі
    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        // Якщо здоров’я впало до нуля або нижче — кінець гри
        if (currentHP <= 0)
        {
            currentHP = 0;
            GameOver();
        }

        UpdateUI();
    }

    // Оновлення всіх елементів UI після зміни HP
    private void UpdateUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHP;

        if (healthText != null)
            healthText.text = currentHP + " / " + maxHP;

        if (fillImage != null)
        {
            // Зміна кольору в залежності від кількості HP
            float healthPercent = (float)currentHP / maxHP;
            fillImage.color = Color.Lerp(Color.red, Color.green, healthPercent);
        }
    }

    // Метод викликається при закінченні гри
    private void GameOver()
    {
        Debug.Log("Game Over — база знищена!");

        // Зміна фону на червоний
        if (backgroundImage != null)
            backgroundImage.color = gameOverColor;

        // Зміна заливки слайдера на червону
        if (fillImage != null)
            fillImage.color = gameOverColor;

        // Показ панелі поразки
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}

        // Вимкнути всі кнопки, які не належать пан
