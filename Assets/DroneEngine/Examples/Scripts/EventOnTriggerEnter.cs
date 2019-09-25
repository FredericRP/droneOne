using FredericRP.EventManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOnTriggerEnter : MonoBehaviour
{
  [SerializeField]
  GameEvent gameEvent;

  private void OnTriggerEnter(Collider other)
  {
    gameEvent.Raise();
  }
}
