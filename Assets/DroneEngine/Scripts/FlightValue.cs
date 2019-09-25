using UnityEngine;

[CreateAssetMenu(menuName = "FredericRP/Drone Engine/Flight Value")]
public class FlightValue : ScriptableObject
{
  public float power;
  /// <summary>
  /// X axis: Pitch (tangage)
  /// </summary>
  public float pitch;
  /// <summary>
  /// Y axis : Yaw (lacet)
  /// </summary>
  public float yaw;
  /// <summary>
  /// Z axis : Roll (roulis)
  /// </summary>
  public float roll;
}
