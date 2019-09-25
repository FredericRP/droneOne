using TMPro;
using UnityEngine;

public class DisplayPitch : MonoBehaviour
{
  [SerializeField]
  FlightValue value;
  [SerializeField]
  TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
      text.text = value.pitch.ToString("F0");
    }
}