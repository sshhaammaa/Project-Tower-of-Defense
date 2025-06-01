using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject objectPrefab;
    public int maxPlaceCount = 5;

    private int currentPlaced = 0;
    private bool isPlacing = false;

    public List<GameObject> placedObjects = new List<GameObject>();

    private GameObject previewObject;

    public void StartPlacing()
    {
        if (currentPlaced < maxPlaceCount)
        {
            isPlacing = true;

            // Створити копію танка для перегляду
            previewObject = Instantiate(objectPrefab);
            SetLayerRecursively(previewObject, LayerMask.NameToLayer("Ignore Raycast")); // щоб не блокував клік
            SetTransparent(previewObject); // зробити прозорим
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
            MovePreviewToMouse();

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // 🔴 Перевіряємо, чи об'єкт під курсором не є "Path"
                    if (hit.collider.CompareTag("Path"))
                    {
                        Debug.Log("Не можна ставити на дорогу!");
                        return;
                    }

                    GameObject newObject = Instantiate(objectPrefab, hit.point, Quaternion.identity);
                    newObject.AddComponent<TankClickDelete>().placer = this;

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