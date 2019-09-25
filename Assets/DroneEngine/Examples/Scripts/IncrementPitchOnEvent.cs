using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementPitchOnEvent : MonoBehaviour
{
  [SerializeField]
  FlightValue value;

  // Update is called once per frame
  public void IncrementPitch()
  {
    value.pitch++;
  }
}
