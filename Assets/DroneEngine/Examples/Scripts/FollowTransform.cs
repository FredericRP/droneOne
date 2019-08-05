using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField]
    Transform sourceTransform;

    // Update is called once per frame
    void Update()
    {
        transform.position = sourceTransform.position;
        // copy only Y rotation
        Vector3 newEulerAngles = sourceTransform.eulerAngles;
        newEulerAngles.Scale(Vector3.up);
        transform.eulerAngles = newEulerAngles;
    }
}
