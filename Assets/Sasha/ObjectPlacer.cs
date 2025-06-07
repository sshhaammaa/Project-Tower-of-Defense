using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour
{
    // Масив префабів башт
    public GameObject[] towerPrefabs;

    // Вартість кожної башти відповідно до індексу
    public int[] towerCosts;

    // Поточний вибраний префаб і його вартість
    private GameObject objectPrefab;
    private int currentCost;

    // Максимальна кількість розміщених башт
    public int maxPlaceCount = 5;

    // Лічильник вже розміщених башт
    private int currentPlaced = 0;

    // Чи активний режим розміщення
    private bool isPlacing = false;

    // Чи активний режим видалення
    public bool isDeleting = false;

    // Список усіх розміщених об'єктів
    public List<GameObject> placedObjects = new List<GameObject>();

    // Об'єкт-привид (для попереднього перегляду перед розміщенням)
    private GameObject previewObject;

    // Вибір башти за індексом і запуск розміщення
    public void SelectAndPlaceTower(int index)
    {
        if (index >= 0 && index < towerPrefabs.Length)
        {
            objectPrefab = towerPrefabs[index];
            currentCost = (index < towerCosts.Length) ? towerCosts[index] : 0;
            StartPlacing();
        }
    }

    // Початок процесу розміщення башти
    public void StartPlacing()
    {
        if (objectPrefab == null)
        {
            Debug.LogError("objectPrefab не вибрано!");
            return;
        }

        if (currentPlaced >= maxPlaceCount)
        {
            Debug.Log("Досягнуто максимуму об’єктів!");
            return;
        }

        // Перевірка, чи вистачає грошей
        if (!PlayerMonety.instance.SpendMoney(currentCost))
        {
            Debug.Log("Не вистачає грошей!");
            return;
        }

        isPlacing = true;

        // Створення об'єкта-привида
        previewObject = Instantiate(objectPrefab);
        SetLayerRecursively(previewObject, LayerMask.NameToLayer("Ignore Raycast"));
        SetTransparent(previewObject);
        DisableScriptsOnPreview(previewObject);
    }

    void Update()
    {
        if (isPlacing)
        {
            // Переміщення об'єкта-привида за мишкою
            MovePreviewToMouse();

            // Обертання об'єкта клавішами Q / E
            if (Input.GetKeyDown(KeyCode.Q))
                previewObject.transform.Rotate(Vector3.up, -45f);
            else if (Input.GetKeyDown(KeyCode.E))
                previewObject.transform.Rotate(Vector3.up, 45f);

            // Клік мишкою для розміщення
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Path"))
                    {
                        Debug.Log("Не можна ставити на дорогу!");
                        return;
                    }

                    // Створення реальної башти на сцені
                    GameObject newObject = Instantiate(objectPrefab, hit.point, previewObject.transform.rotation);

                    // Додаємо до списку і збільшуємо лічильник
                    placedObjects.Add(newObject);
                    currentPlaced++;

                    // Завершення розміщення
                    isPlacing = false;
                    Destroy(previewObject);
                }
            }
        }
    }

    // Переміщення об'єкта-привида під мишкою
    void MovePreviewToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            previewObject.transform.position = hit.point;

            // Колір підказки — червоний для "Path", зелений для дозволеного місця
            if (hit.collider.CompareTag("Path"))
                SetPreviewColor(Color.red, 0.4f);
            else
                SetPreviewColor(Color.green, 0.4f);
        }
    }

    // Задає колір об'єкта-привида
    void SetPreviewColor(Color color, float alpha = 0.4f)
    {
        foreach (var rend in previewObject.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in rend.materials)
            {
                Color c = color;
                c.a = alpha;
                mat.color = c;
            }
        }
    }

    // Вимикає всі скрипти на об'єкті-привиді, щоб він не взаємодіяв з навколишнім середовищем
    void DisableScriptsOnPreview(GameObject obj)
    {
        foreach (var script in obj.GetComponentsInChildren<MonoBehaviour>())
        {
            script.enabled = false;
        }
    }

    // Видалення розміщеного об'єкта з гри
    public void RemovePlacedObject(GameObject obj)
    {
        if (placedObjects.Contains(obj))
        {
            placedObjects.Remove(obj);
            currentPlaced--;
        }
    }

    // Перемикає режим видалення
    public void ToggleDeleteMode()
    {
        isDeleting = !isDeleting;
        Debug.Log("Delete Mode: " + isDeleting);
    }

    // Робить об'єкт прозорим (для попереднього перегляду)
    void SetTransparent(GameObject obj)
    {
        foreach (var rend in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in rend.materials)
            {
                Color c = mat.color;
                c.a = 0.4f;
                mat.color = c;
                mat.SetFloat("_Mode", 2);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            }
        }
    }

    // Рекурсивно змінює шар для всіх дочірніх об’єктів
    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

}
