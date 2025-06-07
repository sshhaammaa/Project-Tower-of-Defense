using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private int hp;
    [SerializeField] private int coinsToDie;

    private bool isDead = false;

    void Start()
    {
        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        StartCoroutine(InitializeMovement());
    }

    private IEnumerator InitializeMovement()
    {
        // Затримка на 1 кадр (можна більше, якщо треба)
        yield return null;

        if (navMeshAgent != null && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.SetDestination(navmesh.Instance.Finish.position);
            navMeshAgent.avoidancePriority = Random.Range(1, 100);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} не на NavMesh! Не вдалося встановити ціль.");
        }
    }


    void Update()
    {
        Vector3 direction = navMeshAgent.velocity;
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, navMeshAgent.angularSpeed * Time.deltaTime);
        }
    }

    public bool GetDamage(int damage)
    {
        if (isDead) return false; 

        hp -= damage;
        Debug.Log("Моб отримав шкоду: " + damage + ". Поточне HP: " + hp);

        if (hp <= 0)
        {
            isDead = true;
            PlayerMonety.instance.AddMoney(coinsToDie); 
            navmesh.Instance.RemoveEnemy(gameObject);
            Destroy(gameObject);
            return true;
        }

        return false;
    }

    public void KillSilently()
    {
        if (isDead) return;

        isDead = true;
        navmesh.Instance.RemoveEnemy(gameObject);
        Destroy(gameObject); 
    }
}
