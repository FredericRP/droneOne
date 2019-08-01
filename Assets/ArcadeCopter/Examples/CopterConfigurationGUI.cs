using UnityEngine;

public class CopterConfigurationGUI : MonoBehaviour {
	
	public ArcadeCopter copter;
	Vector3 position;
	
	void Start() {
		position = copter.transform.position;
	}
	
	// Update is called once per frame
	void OnGUI () {
		GUI.Label(new Rect(Screen.width/2-300, Screen.height-60, 600, 40), "Controls ==> Left/Right : move left/right | Up/Down : move forward/backward | Space = lift up | Control = lift down");
        // | mouse on the left side : click+drag = move
        if (GUI.Button(new Rect(20, 20, 80, 20), "Reset")) {
			copter.GetComponent<DontGoThroughThings>().enabled = false;
			copter.transform.position = position;
			copter.GetComponent<DontGoThroughThings>().InitPosition();
			copter.GetComponent<DontGoThroughThings>().enabled = true;
		}
		GUI.BeginGroup(new Rect(20, 40, 800, 400));
		copter.verticalSpeed = FloatGUI("Vertical Speed", copter.verticalSpeed, 0, 50);
		copter.verticalTransitionSpeed = FloatGUI("Vertical transition speed", copter.verticalTransitionSpeed, 0.01f, 1000);
		copter.horizontalSpeed = FloatGUI("Horizontal Speed", copter.horizontalSpeed, 1, 100);
		//copter.rotationSpeed = FloatGUI("Rotation Speed", copter.rotationSpeed, 0.01f, 10);
		//copter.pitchSpeed = FloatGUI("Pitch Ratio", copter.pitchSpeed, 0, 100);
		//copter.rollSpeed = FloatGUI("Roll Ratio", copter.rollSpeed, 0, 100);
		//copter.groundSpeedRatio = FloatGUI("Grounded Speed Ratio", copter.groundSpeedRatio);
		
        /*
		GUILayout.BeginHorizontal();
		copter.autoStabilize = GUILayout.Toggle(copter.autoStabilize, "Auto stabilize (beta)", GUILayout.Width(100));
		copter.autoStabilizeHeight = FloatGUI("@ Height", copter.autoStabilizeHeight, 10, 100);
		GUILayout.EndHorizontal();
        // */
		
		GUI.EndGroup();
	}
	
	float FloatGUI(string title, float value) {
		return FloatGUI(title, value, 0, 1);
	}
	
	float FloatGUI(string title, float value, float min, float max) {
		GUILayout.BeginHorizontal();
		GUILayout.Label(title);
		string outS = GUILayout.TextField("" + value);
		float parsedValue = System.Single.Parse(outS);
		float newValue = GUILayout.HorizontalSlider(value, min, max, GUILayout.Width(100));
		GUILayout.EndHorizontal();
		float result = (parsedValue != value ? parsedValue : (newValue != value ? newValue : value));
		return result;
	}
	
}
