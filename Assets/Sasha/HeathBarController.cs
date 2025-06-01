using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthBarController : MonoBehaviour
{
    public Slider healthSlider;
    public TextMeshProUGUI hpText;
    public GameObject gameOverText;
    public GameObject restartButton;
    public GameObject gameplayButtons;     // група кнопок
    public GameObject backgroundPanel;     // панель для червоного фону

    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        UpdateHPText();

        gameOverText.SetActive(false);
        restartButton.SetActive(false);
        backgroundPanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TakeDamage(10f);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthSlider.value = currentHealth;
        UpdateHPText();

        if (currentHealth <= 0f)
        {
            ShowGameOver();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthSlider.value = currentHealth;
        UpdateHPText();
    }

    private void UpdateHPText()
    {
        hpText.text = Mathf.RoundToInt(currentHealth) + " / " + Mathf.RoundToInt(maxHealth);
    }

    private void ShowGameOver()
    {
        gameOverText.SetActive(true);
        restartButton.SetActive(true);
        backgroundPanel.SetActive(true); // зробити фон червоним
        gameplayButtons.SetActive(false); // сховати всі інші кнопки
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
    }
}