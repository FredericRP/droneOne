using UnityEngine;
using System.Collections;

namespace FredericRP.DroneEngine
{
    public class TouchInput : MonoBehaviour
    {

        [SerializeField]
        DroneController copter;

        // Touch Devices :
        // Ratio for horizontal move (left/right/fwd/bkwd)
        public float horizontalRatio = 0.2f;
        // Ratio for vertical move (up)
        public float verticalRatio = 0.2f;
        // Ratio for forward move
        public float forwardRatio = 0.4f;
        public float backwardStrafeRatio = 0.3f;


        #region visual feedback
        public bool showArrows;
        public Texture2D fourArrows;
        Vector2 fourArrowSize;
        #endregion


#if UNITY_ANDROID || UNITY_IPHONE || UNITY_IPAD
	// TODO : remove SJoystick references
	public SJoystick directionJoystick;
	public SJoystick engineJoystick;
#endif

        public class CopterFinger
        {
            public int id;
            public Vector2 startPosition;
            public Vector2 currentPosition;
        }

        CopterFinger directionFinger;
        CopterFinger engineFinger;

#if UNITY_ANDROID || UNITY_IPHONE || UNITY_IPAD
	// Use this for initialization
	void Start () {
		directionFinger = new CopterFinger();
		engineFinger = new CopterFinger();

		fourArrowSize = new Vector2(fourArrows.width, fourArrows.height);

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 forwardProjected = transform.forward;
		forwardProjected.y = 0;
		Vector3 rightProjected = transform.right;
		rightProjected.y = 0;

		float horizontal = 0;
		float forward = 0;
		copter.vertical = 0;

		if (directionJoystick) {
			horizontal = directionJoystick.GetAxis().x;
			copter.vertical = engineJoystick.GetAxis().y;
			forward = directionJoystick.GetAxis().y;
			if (forward != 0 || horizontal != 0) {
				copter.targetDirection = forwardProjected * forward + rightProjected * horizontal;
			} else {
				copter.targetDirection = Vector3.zero;
			}
		} else if (Input.touchCount > 0) {
			for (int i=0;i<Input.touchCount;i++) {
				ProcessFinger(Input.GetTouch(i));
			}
			if (directionFinger.id != -1) {
				Vector3 touchDirection = directionFinger.currentPosition - directionFinger.startPosition;
				Vector3 strafe = touchDirection.x * horizontalRatio * rightProjected / Screen.width;
				Vector3 flyForward = touchDirection.y * forwardRatio * forwardProjected / Screen.height;
				if (touchDirection.y < 0)
					forward *= backwardStrafeRatio;
				copter.targetDirection = strafe + flyForward;
			}
			if (engineFinger.id != -1) {
				copter.vertical = ((engineFinger.currentPosition-engineFinger.startPosition).y*verticalRatio/Screen.height);
			} else {
				copter.vertical = 0;
			}
		} else {
			engineFinger.id = -1;
			directionFinger.id = -1;
			// Reset direction only if no input (check also keys)
			if (horizontal==0 && forward==0)
				copter.targetDirection = Vector3.zero;
		}
	}

	void ProcessFinger(Touch finger) {
		switch(finger.phase) {
		case TouchPhase.Canceled:
		case TouchPhase.Ended: Debug.Log ("Cancel/Ended " + finger.fingerId); if (finger.fingerId == directionFinger.id) directionFinger.id = -1; else if (finger.fingerId == engineFinger.id) engineFinger.id = -1; break;
		case TouchPhase.Began:
			Debug.Log ("Begin " + finger.fingerId + " x=" + finger.position.x + " vs. " + (Screen.width / 2));
			if (finger.position.x < Screen.width / 2)
				SetFinger(directionFinger, finger);
			else
				SetFinger(engineFinger, finger);
			break;
		case TouchPhase.Moved:
			if (finger.fingerId == directionFinger.id)
				SetFinger(directionFinger, finger);
			else if (finger.fingerId == engineFinger.id)
				SetFinger(engineFinger, finger);break;
		}
	}
	
	void SetFinger(CopterFinger setFinger, Touch finger) {
		// Check if there is already a finger attached to engine
		if (setFinger.id == -1) {
			setFinger.id = finger.fingerId;
			setFinger.startPosition = finger.position;
		}
		if (setFinger.id == finger.fingerId)
			setFinger.currentPosition = finger.position;
	}

	void OnGUI() {
		if (!showArrows)
			return;

		if (!directionJoystick && directionFinger.id != -1) {
			// Draw the four arrows
			GUI.DrawTexture(new Rect(directionFinger.currentPosition.x-fourArrowSize.x/2, Screen.height-directionFinger.currentPosition.y-fourArrowSize.y/2, fourArrowSize.x*2, fourArrowSize.y*2), fourArrows);
		}
	}
#endif
    }
}