using UnityEngine;

public class TankClickDelete : MonoBehaviour
{
    public ObjectPlacer placer;

    void OnMouseDown()
    {
        Debug.Log("Натиснули на танк: " + gameObject.name);

        if (placer != null && placer.isDeleting)
        {
            Debug.Log("Режим видалення активний, видаляємо...");

            placer.RemovePlacedObject(gameObject);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Режим видалення не активний або placer null");
        }
    }
}
