using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject[] towerPrefabs;     // Префаби башт
    public int[] towerCosts;              // Ціни для кожної башти
    private GameObject objectPrefab;      // Поточний префаб
    private int currentCost;              // Вартість поточної башти

    public int maxPlaceCount = 5;

    private int currentPlaced = 0;
    private bool isPlacing = false;
    public bool isDeleting = false;

    public List<GameObject> placedObjects = new List<GameObject>();
    private GameObject previewObject;

    public void SelectAndPlaceTower(int index)
    {
        if (index >= 0 && index < towerPrefabs.Length)
        {
            objectPrefab = towerPrefabs[index];
            currentCost = (index < towerCosts.Length) ? towerCosts[index] : 0;
            StartPlacing();
        }
    }

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

        if (!PlayerMonety.instance.SpendMoney(currentCost))
        {
            Debug.Log("Не вистачає грошей!");
            return;
        }

        isPlacing = true;

        previewObject = Instantiate(objectPrefab);
        SetLayerRecursively(previewObject, LayerMask.NameToLayer("Ignore Raycast"));
        SetTransparent(previewObject);
        DisableScriptsOnPreview(previewObject);
    }

    void Update()
    {
        if (isPlacing)
        {
            MovePreviewToMouse();

            if (Input.GetKeyDown(KeyCode.Q))
                previewObject.transform.Rotate(Vector3.up, -45f);
            else if (Input.GetKeyDown(KeyCode.E))
                previewObject.transform.Rotate(Vector3.up, 45f);

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

            if (hit.collider.CompareTag("Path"))
                SetPreviewColor(Color.red, 0.4f);
            else
                SetPreviewColor(Color.green, 0.4f);
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

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
