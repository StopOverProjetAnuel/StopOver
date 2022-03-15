using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	private static ObjectPool instance;

	[SerializeField] Pool pool;
	Dictionary<string, List<GameObject>> objectPool;

	private void Awake ()
	{
		InitDictionary();
	}

	/// <summary>
	/// Remplit le dictionnary avec l'array utilisé par l'asset
	/// </summary>
	private void InitDictionary ()
	{
		objectPool = new Dictionary<string, List<GameObject>>();
		foreach(Pool.PoolArray p in pool.objectPool)
		{
			List<GameObject> items = new List<GameObject>();
			GameObject container = new GameObject(p.ID);
			container.transform.parent = transform;

			for(int i = 0; i < p.initialQuantity; i++)
			{
				items.Add(SpawnPrefab(p, container.transform));
				//Debug.Log("Items  " + items.Count);
			}

			objectPool.Add(p.ID, items);
		}
	}

	/// <summary>
	/// Spawns prefab from the PoolArray
	/// </summary>
	/// <param name="p">Pool Array used to spawn target Object</param>
	/// <param name="t">Transform used to parent spawned Object</param>
	/// <returns>Spawned GameObject</returns>
	GameObject SpawnPrefab (Pool.PoolArray p, Transform t)
	{
		if (p.prefab == null)
		{
			Debug.LogError("No prefab set !", gameObject);
			return null;
		}

		GameObject g = Instantiate(p.prefab);

		InitPoolable(g, p.ID);

		g.SetActive(false);
		g.transform.parent = t;

		return g;
	}

	/// <summary>
	/// Initializes a poolable item, setting its ID according to the Pool asset
	/// </summary>
	/// <param name="g">Instantiated GameObject we need to initialize</param>
	/// <param name="ID">ID we set</param>
	void InitPoolable(GameObject g, string ID)
	{
		PoolableItem pool = g.GetComponent<PoolableItem>();

		if(pool)
		{
			pool.ID = ID;
		}
	}

	/// <summary>
	/// Tries to find the pool we need to spawn the new GameObject from, and parents it to the right container
	/// </summary>
	/// <param name="ID">ID used for pooling the object</param>
	public void SpawnPrefabFromPool(string ID)
	{
		foreach (Pool.PoolArray p in pool.objectPool)
		{
			if(p.ID == ID)
			{
				Transform t = transform;
				int childCount = transform.childCount;

				//Debug.Log("Child count : " + childCount);
				for(int i = 0; i < childCount; i++)
				{
					Transform child = transform.GetChild(i);
					if (child.gameObject.name == ID)
					{
						t = child;
						break;
					}
				}
				objectPool[p.ID].Add(SpawnPrefab(p, t));
			}
		}
	}

	public GameObject GetFromPool(string ID, Transform t)
	{
		// If no item is available from the pool
		if(objectPool[ID].Count == 0)
		{
			SpawnPrefabFromPool(ID);
		}

		//Gets the last GameObject from the pool (First In First Out)
		GameObject g = objectPool[ID][0];

		g.SetActive(true);
		g.transform.position = t.position;
		g.transform.rotation = t.rotation;

		objectPool[ID].RemoveAt(0);

		return g;
	}

	/// <summary>
	/// Return pooled Object to the corresponding pool
	/// </summary>
	/// <param name="ID">ID used by the object</param>
	/// <param name="g"></param>
	public void ReturnToPool(string ID, GameObject g)
	{
		g.SetActive(false);
		objectPool[ID].Add(g);
	}

	public static ObjectPool Instance
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<ObjectPool>();

				if(instance == null)
				{
					GameObject container = new GameObject("Object Pooler");
					instance = container.AddComponent<ObjectPool>();
				}
			}

			return instance;
		}
	}

}
