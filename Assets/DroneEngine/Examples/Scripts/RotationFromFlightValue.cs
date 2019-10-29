using UnityEngine;

public class RotationFromFlightValue : MonoBehaviour
{
    [SerializeField]
    FlightValue value;
    [SerializeField]
    FlightValue xMultiplier;
    [SerializeField]
    FlightValue yMultiplier;
    [SerializeField]
    FlightValue zMultiplier;
    [SerializeField]
    float overallRatio = 90;

  #region runtime vars
    Vector3 eulerAngles;
  #endregion

  private void Start()
    {
        eulerAngles = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        float x = value.power * xMultiplier.power + value.pitch * xMultiplier.pitch + value.yaw * xMultiplier.yaw + value.roll * xMultiplier.roll;
        float y = value.power * yMultiplier.power + value.pitch * yMultiplier.pitch + value.yaw * yMultiplier.yaw + value.roll * yMultiplier.roll;
        float z = value.power * zMultiplier.power + value.pitch * zMultiplier.pitch + value.yaw * zMultiplier.yaw + value.roll * zMultiplier.roll;
        eulerAngles.Set(x, y, z);
        transform.localEulerAngles = eulerAngles * overallRatio;
    }
}
