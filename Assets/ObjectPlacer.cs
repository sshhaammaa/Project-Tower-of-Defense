using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject objectPrefab;
    public int maxPlaceCount = 5;

    private int currentPlaced = 0;
    private bool isPlacing = false;
    public bool isDeleting = false;

    public List<GameObject> placedObjects = new List<GameObject>();

    private GameObject previewObject;

    public void StartPlacing()
    {
        if (currentPlaced < maxPlaceCount)
        {
            isPlacing = true;

            int cost = 25;
            if (!PlayerMonety.instance.SpendMoney(cost))
            {
                Debug.Log("Не вистачає грошей!");
                return;
            }
            // Створити копію танка для перегляду
            previewObject = Instantiate(objectPrefab);
            
            SetLayerRecursively(previewObject, LayerMask.NameToLayer("Ignore Raycast"));
            SetTransparent(previewObject);
            DisableScriptsOnPreview(previewObject); // 🔽 НОВА ФУНКЦІЯ



        }
        else
        {
            Debug.Log("Досягнуто максимуму об’єктів!");
        }
    }

    void Update()
    {
        if (isPlacing)
        {
            MovePreviewToMouse(); // переміщення прев’ю за мишкою

            //  Обертання прев’ю клавішами Q та E
            if (Input.GetKeyDown(KeyCode.Q))
            {
                previewObject.transform.Rotate(Vector3.up, -45f); // вліво
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                previewObject.transform.Rotate(Vector3.up, 45f); // вправо
            }

            //  ЛКМ для розміщення
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Не ставимо на дорогу
                    if (hit.collider.CompareTag("Path"))
                    {
                        Debug.Log("Не можна ставити на дорогу!");
                        return;
                    }

                    //  Створюємо справжній танк із поточним поворотом
                    GameObject newObject = Instantiate(objectPrefab, hit.point, previewObject.transform.rotation);
                    TankClickDelete deleter = newObject.AddComponent<TankClickDelete>();
                    deleter.placer = this;

                    placedObjects.Add(newObject);
                    currentPlaced++;

                    isPlacing = false;
                    Destroy(previewObject);
                }
            }
        }
    }

    void MovePreviewToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            previewObject.transform.position = hit.point;

            // 🔴 Якщо це дорога — ставимо червоний колір
            if (hit.collider.CompareTag("Path"))
            {
                SetPreviewColor(Color.red, 0.4f);
            }
            else
            {
                SetPreviewColor(Color.green, 0.4f);
            }
        }
    }
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
    void DisableScriptsOnPreview(GameObject obj)
    {
        // Вимикаємо усі MonoBehaviour скрипти на об’єкті
        foreach (var script in obj.GetComponentsInChildren<MonoBehaviour>())
        {
            script.enabled = false;
        }
    }


    public void RemovePlacedObject(GameObject obj)
    {
        if (placedObjects.Contains(obj))
        {
            placedObjects.Remove(obj);
            currentPlaced--;
        }
    }
    public void ToggleDeleteMode()
    {
        isDeleting = !isDeleting;
        Debug.Log("Delete Mode: " + isDeleting);
    }

    // Робимо матеріали прозорими
    void SetTransparent(GameObject obj)
    {
        foreach (var rend in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in rend.materials)
            {
                Color c = mat.color;
                c.a = 0.4f;
                mat.color = c;
                mat.SetFloat("_Mode", 2); // Transparent
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

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

}