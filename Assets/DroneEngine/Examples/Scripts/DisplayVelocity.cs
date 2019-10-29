using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayVelocity : MonoBehaviour
{
  [SerializeField]
  Rigidbody body;
  [SerializeField]
  TextMeshProUGUI text;
  [SerializeField]
  AnimationCurve curve;

    // Update is called once per frame
    void Update()
    {
      text.text = curve.Evaluate(body.velocity.z).ToString("F0");
    }
}
