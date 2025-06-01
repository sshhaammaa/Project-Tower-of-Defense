using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : MonoBehaviour
{
	public float range = 5f;
	public float fireRate = 1f;
	public GameObject projectilePrefab;
	public Transform firePoint;

	private float fireCountdown = 0f;

	void Update()
	{
		fireCountdown -= Time.deltaTime;
		MobMovement target = FindClosestEnemy();

		if (target != null && fireCountdown <= 0f)
		{
			Shoot(target);
			fireCountdown = 1f / fireRate;
		}
	}

	MobMovement FindClosestEnemy()
	{
		MobMovement[] enemies = GameObject.FindObjectsOfType<MobMovement>();
		MobMovement closest = null;
		float shortestDistance = Mathf.Infinity;

		foreach (MobMovement enemy in enemies)
		{
			float dist = Vector3.Distance(transform.position, enemy.transform.position);
			if (dist < shortestDistance && dist <= range)
			{
				shortestDistance = dist;
				closest = enemy;
			}
		}

		return closest;
	}

	void Shoot(MobMovement target)
	{
		GameObject projGO = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
		attack proj = projGO.GetComponent<attack>();
		proj.Seek(target);
	}
}
