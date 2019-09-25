using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayPower : MonoBehaviour
{
  [SerializeField]
  FlightValue value;
  [SerializeField]
  TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
      text.text = value.power.ToString("F0");
    }
}
