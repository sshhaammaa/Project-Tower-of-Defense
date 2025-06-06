using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarManager : MonoBehaviour
{
    public static HealthBarManager instance;

    [Header("UI")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public GameObject gameOverPanel;
    public Image backgroundImage;
    public Image fillImage;
    public Color normalColor = Color.green;
    public Color gameOverColor = Color.red;

    [Header("Настройки бази")]
    public int maxHP = 100;
    private int currentHP;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        currentHP = maxHP;

        // Установлюємо max значення для слайдера
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHP;
            healthSlider.minValue = 0;
        }

        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            currentHP = 0;
            GameOver();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHP;

        if (healthText != null)
            healthText.text = currentHP + " / " + maxHP;

        if (fillImage != null)
        {
            float healthPercent = (float)currentHP / maxHP;
            fillImage.color = Color.Lerp(Color.red, Color.green, healthPercent);
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over — база знищена!");

        if (backgroundImage != null)
            backgroundImage.color = gameOverColor;

        if (fillImage != null)
            fillImage.color = gameOverColor;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Вимикаємо всі кнопки, крім Game Over панелі
        Button[] allButtons = FindObjectsOfType<Button>();
        foreach (Button btn in allButtons)
        {
            if (!gameOverPanel.transform.IsChildOf(btn.transform))
                btn.gameObject.SetActive(false);
        }
    }
}
