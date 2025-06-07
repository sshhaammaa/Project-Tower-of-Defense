using UnityEngine;

public class TowerDeleter : MonoBehaviour
{
    public ObjectPlacer objectPlacer; // ������� � ���������

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
                    Debug.Log("��������� �����: " + towerRoot.name);
                    objectPlacer.RemovePlacedObject(towerRoot);
                    Destroy(towerRoot);
                }
                else
                {
                    Debug.Log("�� �� �����.");
                }
            }
        }
    }

    GameObject FindPlacedTowerRoot(GameObject clickedObject)
    {
        Debug.Log("������� ��: " + clickedObject.name);

        if (objectPlacer.placedObjects.Contains(clickedObject))
        {
            Debug.Log("�������� �� �������� ��'���.");
            return clickedObject;
        }

        Transform current = clickedObject.transform;

        while (current.parent != null)
        {
            current = current.parent;
            Debug.Log("���������� ������: " + current.name);

            if (objectPlacer.placedObjects.Contains(current.gameObject))
            {
                Debug.Log("�������� � placedObjects: " + current.name);
                return current.gameObject;
            }
        }

        Debug.Log("�� �������� � placedObjects.");
        return null;
    }
}
