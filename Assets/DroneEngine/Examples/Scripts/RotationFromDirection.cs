using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFromDirection : MonoBehaviour
{
    [SerializeField]
    Rigidbody body;

    // Update is called once per frame
    void Update()
    {
        // Do not use vertical velocity for rotation
        Vector3 planarVelocity = body.velocity;
        planarVelocity.y = 0;
        // To prevent weird behavior, apply rotation from direction only when moving
        if (planarVelocity.sqrMagnitude > 0)
            transform.localRotation = Quaternion.FromToRotation(Vector3.forward, planarVelocity);   
    }
}
