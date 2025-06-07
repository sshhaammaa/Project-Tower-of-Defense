using UnityEngine;

public class TowerDeleter : MonoBehaviour
{
    public ObjectPlacer objectPlacer; // Признач у інспекторі

    void Update()
    {
        if (objectPlacer != null && objectPlacer.isDeleting && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                GameObject towerRoot = FindPlacedTowerRoot(clickedObject);

                if (towerRoot != null)
                {
                    Debug.Log("Видаляємо башту: " + towerRoot.name);
                    objectPlacer.RemovePlacedObject(towerRoot);
                    Destroy(towerRoot);
                }
                else
                {
                    Debug.Log("Це не башта.");
                }
            }
        }
    }

    GameObject FindPlacedTowerRoot(GameObject clickedObject)
    {
        Debug.Log("Клікнуто по: " + clickedObject.name);

        if (objectPlacer.placedObjects.Contains(clickedObject))
        {
            Debug.Log("Знайдено як основний об'єкт.");
            return clickedObject;
        }

        Transform current = clickedObject.transform;

        while (current.parent != null)
        {
            current = current.parent;
            Debug.Log("Перевіряємо батька: " + current.name);

            if (objectPlacer.placedObjects.Contains(current.gameObject))
            {
                Debug.Log("Знайдено в placedObjects: " + current.name);
                return current.gameObject;
            }
        }

        Debug.Log("Не знайдено в placedObjects.");
        return null;
    }
}
