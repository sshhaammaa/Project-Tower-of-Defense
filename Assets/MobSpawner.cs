using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Компонент, що відповідає за спавн хвиль ворогів
public class WaveSpawner : MonoBehaviour
{
    // Структура для опису однієї хвилі ворогів
    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;     // Префаб ворога
        public int enemyCount;             // Кількість ворогів у хвилі
        public float spawnInterval = 1f;   // Інтервал між спавном ворогів
    }

    public List<Wave> waves;              // Список усіх хвиль
    public Transform spawnPoint;          // Точка, де будуть з’являтись вороги

    private int currentWaveIndex = 0;     // Індекс поточної хвилі
    private bool isSpawning = false;      // Чи зараз відбувається спавн
    private List<GameObject> aliveEnemies = new List<GameObject>();  // Живі вороги на сцені

    void Update()
    {
        // Якщо не спавнимо, всі вороги мертві і залишились ще хвилі — запускаємо наступну
        if (!isSpawning && aliveEnemies.Count == 0 && currentWaveIndex < waves.Count)
        {
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));  // Запускаємо корутину спавну
            currentWaveIndex++;  // Переходимо до наступної хвилі
        }

        // Видаляємо зі списку всіх ворогів, які вже знищені 
        aliveEnemies.RemoveAll(enemy => enemy == null);
    }

    // Корутина для поетапного спавну ворогів із затримкою
    IEnumerator SpawnWave(Wave wave)
    {
        isSpawning = true;
        Debug.Log($"▶️ Починаємо хвилю {currentWaveIndex + 1}");

        for (int i = 0; i < wave.enemyCount; i++)
        {
            // Створюємо ворога у точці спавну
            GameObject enemy = Instantiate(wave.enemyPrefab, spawnPoint.position, Quaternion.identity);
            aliveEnemies.Add(enemy);  // Додаємо його до списку живих
            yield return new WaitForSeconds(wave.spawnInterval);  // Затримка між спавном
        }

        isSpawning = false;  // Коли всі вороги з’явились, спавн завершено
    }
}
