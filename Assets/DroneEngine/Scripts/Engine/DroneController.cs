using UnityEngine;

namespace FredericRP.DroneEngine
{
  public class DroneController : MonoBehaviour
  {
    [SerializeField]
    float maxAngleToLift = 85;

    [SerializeField]
    DroneConfiguration droneConfiguration;
    [SerializeField]
    FlightValue flightValue;

    Rigidbody body;

    /// <summary>
    /// Input multiplier for vertical velocity brake
    /// </summary>
    public float verticalVelocityRatio = 0.1f;
    /// <summary>
    /// If drone is lifting something, allows to set the entire mass (add the drone mass)
    /// </summary>
    public float massToLift = 1.6f;

    public bool useBodyMass = true;

    #region runtime vars
    float verticalForce;

    public bool isGrounded;

    #endregion

    // TODO make it SO float value
    public float RotorSpeed { get { return verticalForce; } }

    // Use this for initialization
    void Start()
    {
      body = GetComponent<Rigidbody>();
      if (body == null)
      {
        Debug.LogError("FlyEngine game object must have a rigidbody attached !");
        enabled = false;
        return;
      }

      if (useBodyMass)
        massToLift = body.mass;

      verticalForce = 0;
    }

    // Update is called once per frame
    void Update()
    {
      // Autopilot : easy drive
      if (droneConfiguration.easyDrive && flightValue.roll != 0)
      {
        flightValue.pitch = (flightValue.pitch + flightValue.roll * droneConfiguration.rollToPitchRatio) / 2;
        flightValue.yaw = flightValue.roll * droneConfiguration.rollToYawRatio;
      }

      // GENERAL ENGINE
      Vector3 targetAngle = transform.localEulerAngles;
      // X axis : Pitch (tangage)
      targetAngle.x += flightValue.pitch * Time.deltaTime * droneConfiguration.pitchSpeed;
      // Y axis : Yaw (lacet)
      targetAngle.y += flightValue.yaw * Time.deltaTime * droneConfiguration.yawSpeed;
      // Z axis : Roll (roulis)
      targetAngle.z += flightValue.roll * Time.deltaTime * droneConfiguration.rollSpeed;
      // Power
      verticalForce = Mathf.Lerp(verticalForce, Mathf.Clamp01(flightValue.power) * droneConfiguration.maxPower, Time.deltaTime * droneConfiguration.powerSpeed);

      // LIMITS
      if (droneConfiguration.maxPitchAngle != 0)
      {
        while (targetAngle.x >= 180)
          targetAngle.x -= 360;
        while (targetAngle.x <= -180)
          targetAngle.x += 360;
        targetAngle.x = Mathf.Clamp(targetAngle.x, -droneConfiguration.maxPitchAngle, droneConfiguration.maxPitchAngle);
      }
      if (droneConfiguration.maxRollAngle != 0)
      {
        while (targetAngle.z >= 180)
          targetAngle.z -= 360;
        while (targetAngle.z <= -180)
          targetAngle.z += 360;
        targetAngle.z = Mathf.Clamp(targetAngle.z, -droneConfiguration.maxRollAngle, droneConfiguration.maxRollAngle);
      }

      // AUTO PILOT

      // Auto stability force to have the local Y axis up, without changing the actual yaw (Y)
      if (flightValue.roll == 0) {
        targetAngle.z = Mathf.Lerp(targetAngle.z, 0, droneConfiguration.horizontalAutoStability * Time.deltaTime);
      }

      if (flightValue.pitch == 0)
      {
        targetAngle.x = Mathf.Lerp(targetAngle.x, 0, droneConfiguration.horizontalAutoStability * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.eulerAngles.y, 0),
            //droneConfiguration.horizontalAutoStability * Time.deltaTime);
      }
      // set to limits if any
      transform.localEulerAngles = targetAngle;

      // Maintain height if stable enough and no power is requested by user
      float angleToUp = Vector3.Angle(transform.up, Vector3.up);
      if (droneConfiguration.maintainHeight && flightValue.power == 0 && angleToUp < maxAngleToLift)
      {
        float forceToMaintainHeight = massToLift * -Physics.gravity.y;
        forceToMaintainHeight -= body.velocity.y * verticalVelocityRatio;
        // Take into account angle between world Vector3.up and transform.up to increase vertical force to maintain height
        // Quaternion.Angle returns degrees but Mathf.Cos uses radians dafuck
        verticalForce = forceToMaintainHeight / Mathf.Cos(angleToUp * Mathf.Deg2Rad);
      }
    }

    void FixedUpdate()
    {
      if (verticalForce != 0)
      {
        body.AddForce(transform.up * verticalForce);
      }
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.green;
      Gizmos.DrawLine(transform.position, transform.position + transform.up * verticalForce);
    }
  }
}