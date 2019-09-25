using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDistanceFromZ : MonoBehaviour
{
  [SerializeField]
  FlightValue distance;

    // Update is called once per frame
    void Update()
    {
      distance.power = transform.position.z;
    }
}
