using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
  [SerializeField]
  GameObject[] prefabList;
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
    GameObject go = Instantiate(prefabList[index++], parent);
    go.transform.localPosition = offset;
    offset += prefabSize;
    if (index >= prefabList.Length)
      index = 0;
  }
}
