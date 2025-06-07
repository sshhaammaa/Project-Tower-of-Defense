using UnityEngine;

public class TankClickDelete : MonoBehaviour
{
    public ObjectPlacer placer;

    void OnMouseDown()
    {
        Debug.Log("��������� �� ����: " + gameObject.name);

        if (placer != null && placer.isDeleting)
        {
            Debug.Log("����� ��������� ��������, ���������...");

            placer.RemovePlacedObject(gameObject);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("����� ��������� �� �������� ��� placer null");
        }
    }
}
