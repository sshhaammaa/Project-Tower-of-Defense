using UnityEngine;

public class BaseZone : MonoBehaviour
{
    public int damageToBase = 10;

    private void OnTriggerEnter(Collider other)
    {
        MobMovement mob = other.GetComponent<MobMovement>();
        if (mob != null)
        {
            HealthBarManager.instance.TakeDamage(damageToBase);
            mob.KillSilently(); // 🧨 вбиває без нагороди
        }
    }
}
