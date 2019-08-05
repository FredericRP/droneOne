using UnityEngine;

namespace FredericRP.DroneEngine
{
    [CreateAssetMenu(fileName = "DroneConfig", menuName = "FredericRP/Drone Engine/Configuration")]
    public class DroneConfiguration : ScriptableObject
    {
        [Header("Speed transition")]
        [Tooltip("X axis: Pitch (tangage)")]
        public float pitchSpeed = 200;
        [Tooltip("Y axis : Yaw (lacet)")]
        public float yawSpeed = 200;
        [Tooltip("Z axis : Roll (roulis)")]
        public float rollSpeed = 200;
        public float powerSpeed = 200;
        [Header("Max rotor speed")]
        public float maxPower = 20;

        [Header("Limits (set to 0 for none)")]
        public float maxPitchAngle = 0;
        //public float maxYawAngle = 0;
        public float maxRollAngle = 0;

        [Header("Autopilot parameters")]
        public bool maintainHeight = false;
        //[Tooltip("Like a GPS : try to keep position above point")]
        //public bool maintainPosition = false;
        public float freeDragRatio = 2;
        [Tooltip("Back to horizontal orientation automatically")]
        public float horizontalAutoStability = 1;
        // easy drive : change yaw and pitch from roll
        public bool easyDrive = false;
        public float rollToYawRatio = 0.5f;
        public float rollToPitchRatio = -0.5f;
    }
}