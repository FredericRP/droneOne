using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTriggerEnter : MonoBehaviour
{
  [SerializeField]
  GameObject target;

  private void Start()
  {
    if (target == null)
      target = gameObject;
  }

  void OnTriggerEnter(Collider collider)
  {
    Destroy(target);
  }
}
