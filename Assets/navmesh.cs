using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class navmesh : MonoBehaviour
{
	public static navmesh Instance;

	public Transform Finish;

	[SerializeField] private float startSpawnMob;
	[SerializeField] private List<MobWave> mobWaves = new List<MobWave>();

	[SerializeField] private GameObject panel;
	[SerializeField] private GameObject button;
	private List<GameObject> prefabsInGame = new List<GameObject>();

	private int id;
	private float timeSpawnSpeed;
	private int mobSpawnLength;
	private bool startWave = false;
	private bool start = false;
	private bool end = false;

	void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		if (start)
		{
			if (!end)
			{
				startSpawnMob -= Time.deltaTime;

				if (startSpawnMob < 0)
				{
					MobWave mobWave = mobWaves[id];

					if (!startWave)
					{
						timeSpawnSpeed = mobWave.TimeSpawnSpeed;
						startWave = true;
					}
					else
					{
						timeSpawnSpeed -= Time.deltaTime;

						if (timeSpawnSpeed < 0)
						{
							prefabsInGame.Add(Instantiate(
								mobWave.prefab,
								transform.position,
								Quaternion.identity
							));

							mobSpawnLength++;

							if (mobSpawnLength >= mobWave.MobSpawnLength)
							{
								mobSpawnLength = 0;
								id++;

								if (id > mobWaves.Count - 1)
								{
									end = true;
								}
								else
								{
									button.SetActive(true);
									start = false;
								}
							}

							startWave = false;
						}
					}
				}
			}
			else
			{
				if (prefabsInGame.Count == 0)
				{
					panel.SetActive(true);
				}
			}
		}
	}

	public void RemoveEnemy(GameObject gameObject)
	{
		prefabsInGame.Remove(gameObject);
	}

	public void StartGame()
	{
		start = true;
	}
}

[Serializable]
public class MobWave
{
	public GameObject prefab;
	public int MobSpawnLength;
	public float TimeSpawnSpeed;
}
