using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField]
    Transform sourceTransform;

    // Update is called once per frame
    void Update()
    {
        transform.position = sourceTransform.position;
    }
}
