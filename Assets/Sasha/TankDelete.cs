using UnityEngine;

public class TankClickDelete : MonoBehaviour
{
    public ObjectPlacer placer;

    void OnMouseDown()
    {
        if (placer != null && placer.isDeleting)
        {
            placer.placedObjects.Remove(gameObject);
            Destroy(gameObject);
            Debug.Log("Танк видалено");
        }
    }
}