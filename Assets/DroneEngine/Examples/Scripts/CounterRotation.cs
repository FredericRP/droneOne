using UnityEngine;

public class CounterRotation : MonoBehaviour
{
    [Tooltip("Visual can reflect only part of the physical orientation")]
    [SerializeField]
    float visualOrientationFactor = 0.5f;
    [SerializeField]
    Transform bodyTransform;

    // Update is called once per frame
    void LateUpdate()
    {
        if (visualOrientationFactor != 1)
        {
            // No change on yaw (Y) axis
            Vector3 invertedRotation = new Vector3(-bodyTransform.eulerAngles.x, 0, -bodyTransform.eulerAngles.z);
            transform.eulerAngles = Vector3.Lerp(Vector3.zero, invertedRotation, visualOrientationFactor);
        }
    }
}
