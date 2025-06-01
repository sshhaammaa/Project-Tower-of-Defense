using UnityEngine;

public class TankClickDelete : MonoBehaviour
{
    public ObjectPlacer placer;

    void OnMouseDown()
    {
        if (placer != null)
        {
            placer.RemovePlacedObject(gameObject);
        }

        Destroy(gameObject);
    }
}