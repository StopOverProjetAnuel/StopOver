using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableItem : MonoBehaviour
{
	[Tooltip("ID of the poolable Item. Only used for reading purposes")]
	public string ID = "";

	private void OnEnable ()
	{
		//Debug.Log("Enable");
		OnObjectPool();
	}

	private void OnDisable ()
	{
		//Debug.Log("Disable");
		OnObjectStore();
	}

	public virtual void OnObjectPool()
	{
		//DU CODE
	}

	public virtual void OnObjectStore ()
	{

	}
}
