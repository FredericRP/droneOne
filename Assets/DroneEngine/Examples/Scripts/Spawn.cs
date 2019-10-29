using FredericRP.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
  [SerializeField]
  string poolId = "pool";
  [SerializeField]
  string[] prefabNameList;
  [SerializeField]
  Vector3 prefabSize;
  [SerializeField]
  Transform parent;

  int index;
  Vector3 offset;

  // Start is called before the first frame update
  void Start()
  {
    index = 0;
    offset = Vector3.zero;
    SpawnPrefab();
    SpawnPrefab();
    SpawnPrefab();
  }

  public void SpawnPrefab()
  {
    GameObject go = ObjectPool.GetObjectPool(poolId).GetFromPool(prefabNameList[index++]);
    go.transform.localPosition = offset;
    offset += prefabSize;
    if (index >= prefabNameList.Length)
      index = 0;
  }
}
