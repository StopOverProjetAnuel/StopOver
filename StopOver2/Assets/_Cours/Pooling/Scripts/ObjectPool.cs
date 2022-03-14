using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	private static ObjectPool instance;

	public Dictionary<string, List<GameObject>> objectPool;
	public Pool pool;

	private void Awake ()
	{
		InitDictionary();
	}

	// On remplit le dictionnary avec l'array utilisé par l'asset
	private void InitDictionary ()
	{
		objectPool = new Dictionary<string, List<GameObject>>();
		foreach(Pool.PoolArray p in pool.objectPool)
		{
			List<GameObject> items = new List<GameObject>();
			for(int i = 0; i < p.initialQuantity; i++)
			{
				Debug.Log("Chibre " + i);

				if(p.prefab == null)
				{
					Debug.LogError("No prefab set !", gameObject);
					return;
				}

				GameObject g = Instantiate(p.prefab);
				g.SetActive(false);
				g.transform.parent = transform;

				items.Add(g);
				Debug.Log("Items  " + items.Count);
			}

			objectPool.Add(p.ID, items);
		}
	}

	public GameObject GetFromPool(string ID, Transform t)
	{
		if(objectPool[ID].Count == 0)
		{
			Debug.LogError("Nique, ajoutez plus de prefabs à spawn");
		}

		GameObject g = objectPool[ID][0];

		g.SetActive(true);
		g.transform.position = t.position;
		g.transform.rotation = t.rotation;

		objectPool[ID].RemoveAt(0);

		return g;
	}

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
