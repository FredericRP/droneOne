using UnityEngine;

namespace FredericRP.DroneEngine
{
  [CreateAssetMenu(fileName = "DroneConfig", menuName = "FredericRP/Drone Engine/Configuration")]
  public class DroneConfiguration : ScriptableObject
  {
    [System.Serializable]
    public class RotationAngleClamp
    {
      public bool clamp = false;
      public float min;
      public float max;
    }

    [Header("Speed transition")]
    [Tooltip("X axis: Pitch (tangage)")]
    public float pitchSpeed = 200;
    [Tooltip("Y axis : Yaw (lacet)")]
    public float yawSpeed = 200;
    [Tooltip("Z axis : Roll (roulis)")]
    public float rollSpeed = 200;
    public float powerSpeed = 200;
    [Header("Rotor power")]
    public float maxPower = 20;
    public float minPower = 2;
    [Header("Power impact on pitch")]
    public float pitchPowerPositiveImpact = 0;
    public float pitchPowerNegativeImpact = 0;

    [Header("Limits (set to 0 for none)")]
    public RotationAngleClamp pitchClamp;
    public RotationAngleClamp yawClamp;
    public RotationAngleClamp rollClamp;

    [Header("Autopilot parameters")]
    public bool maintainHeight = false;
    [Tooltip("Back to horizontal orientation automatically")]
    public float horizontalAutoStability = 1;
    // easy drive : change yaw and pitch from roll
    public bool easyDrive = false;
    public float rollToYawRatio = 0.5f;
    public float rollToPitchRatio = -0.5f;
  }
}