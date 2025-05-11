using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MobMovement : MonoBehaviour
{
	[SerializeField]
	private NavMeshAgent navMeshAgent;

	[SerializeField]
	private int hp;

	[SerializeField]
	private int coinsToDie;

	void Start()
	{
		if (navMeshAgent != null)
		{
			navMeshAgent.SetDestination(navmesh.Instance.Finish.position);
			navMeshAgent.avoidancePriority = Random.Range(1, 100);
		}
	}

	private void Update()
	{
		Vector3 direction = navMeshAgent.velocity.normalized;

		if (direction != Vector3.zero)
		{
			Quaternion toRotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, navMeshAgent.angularSpeed * Time.deltaTime);
		}
	}
	public bool GetDamage(int damage)
	{
		hp -= damage;

		if (hp <= 0)
		{
			//ShopManager.instance.AddCoins(coinsToDie);
			navmesh.Instance.RemoveEnemy(gameObject);
			Destroy(gameObject);
			return true; 
		}

		return false; 
	}
}
