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
      flightValue.pitch = context.action.ReadValue<float>();
    }
    public void OnPower(InputAction.CallbackContext context)
    {
      flightValue.power = context.action.ReadValue<float>();
    }
    public void OnRoll(InputAction.CallbackContext context)
    {
      flightValue.roll = -context.action.ReadValue<float>();
    }
    public void OnYaw(InputAction.CallbackContext context)
    {
      flightValue.yaw = context.action.ReadValue<float>();
    }
  }
}