using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pool", menuName = "Poolable/Pool")]
public class Pool : ScriptableObject
{
    public PoolArray[] objectPool;

	[System.Serializable]
	public class PoolArray
	{
		public string ID = "FX123";

		public GameObject prefab;
		public int initialQuantity = 20;
	}
}
