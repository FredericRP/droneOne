using UnityEngine;
using System.Collections;

public class KeyboardInput : MonoBehaviour {

	[SerializeField]
	ArcadeCopter copter;
    	
	// Update is called once per frame
	void Update () {
		Vector3 forwardProjected = transform.forward;
		forwardProjected.y = 0;
		Vector3 rightProjected = transform.right;
		rightProjected.y = 0;

        // Left joystick / keys : roll(H) + pitch(V)

        // Right joystick / keys : yaw(H) + power(V)

        // Roll (roulis)
		float horizontal = Input.GetAxis("Roll");
        copter.SetRoll(-horizontal);
        // Pitch (tangage)
		float forward = Input.GetAxis("Pitch");
        copter.SetPitch(forward);
        // Yaw (lacet)
        float rotation = Input.GetAxis("Yaw");
        /*
        if (forward != 0 || horizontal != 0) {
			copter.SetHorizontalDirection(forwardProjected * forward + rightProjected * horizontal);
		} else {
			copter.SetHorizontalDirection(Vector3.zero);
		}
        // */
        //copter.SetRotation(rotation);
        copter.SetYaw(rotation);

        // Power
        //SetVerticalInput
        copter.SetPower(Input.GetAxis("Power"));
    }
}
