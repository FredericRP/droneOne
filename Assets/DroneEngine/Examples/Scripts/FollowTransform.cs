using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField]
    Transform sourceTransform;

  Vector3 startOffset;

  private void Start()
  {
    startOffset = transform.position - sourceTransform.position;
  }

  // Update is called once per frame
  void Update()
    {
        transform.position = sourceTransform.position + startOffset;
    }
}
