using UnityEngine;
using UnityEngine.InputSystem;

namespace FredericRP.DroneEngine
{
  public class KeyboardInput : MonoBehaviour
  {
    [SerializeField]
    FlightValue flightValue;

    public void OnPitch(InputAction.CallbackContext context)
    {
      flightValue.pitch = (float)context.action.ReadValue<double>();
    }

    public void OnTruc()
    {

    }

    // Update is called once per frame
    void Update()
    {
      // Left joystick / keys : roll(H) + pitch(V)

      // Right joystick / keys : yaw(H) + power(V)

      // Roll (roulis)
      float roll = Input.GetAxis("Roll");
      // TODO : allow to invert axis in options
      flightValue.roll = -roll;
      // Pitch (tangage)
      float pitch = Input.GetAxis("Pitch");
      flightValue.pitch = pitch;
      // Yaw (lacet)
      float yaw = Input.GetAxis("Yaw");
      flightValue.yaw = yaw;

      // Power
      flightValue.power = Input.GetAxis("Power");
    }
  }
}