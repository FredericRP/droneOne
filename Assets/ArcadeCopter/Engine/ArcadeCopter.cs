using UnityEngine;

public class ArcadeCopter : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] reactorPSList;
    [SerializeField]
    AnimationCurve reactorEmissionCurve;
    //public Vector3 rotorAxis = Vector3.up;
    //public Transform[] rotorList;
    //public float rotorSpeed = 10;
    //public float minRotorSpeed = 10;
    [SerializeField]
    float maxAngleToLift = 85;
    //public float maxAngleForward = 55;
    //public float maxAngleBackward = -35;
    //public float groundSpeedRatio = 0.1f;
    [Tooltip("Visual can reflect only part of the physical orientation")]
    [SerializeField]
    float visualOrientationFactor = 0.5f;
    [SerializeField]
    Transform visualTransform;

    [SerializeField]
    DroneConfiguration droneConfiguration;

    public float rotationSpeed = 3f;
    public float verticalSpeed = 20;
    public float horizontalSpeed = 24;
    /// <summary>
    /// time ratio for vertical force transition
    /// </summary>
    public float verticalTransitionSpeed = 1000;
    /// <summary>
    /// time ratio for horizontal force transition
    /// </summary>
    public float horizontalTransitionSpeed = 1000;
    /// <summary>
    /// Automation : ratio to stay on position once user stops pressing horizontal axis
    /// </summary>
    public float brakeRatio = 2;

    /// <summary>
    /// Automation : force back to original position
    /// </summary>
    public float standUpSpeed = 2;
    /// <summary>
    /// Automation : force back to original position when upside down
    /// </summary>
    public float bottomUpStandUpSpeed = 16;

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

    // Input : target direction
    float yaw, pitch, roll, power;
    Vector3 targetDirection;
    Vector3 horizontalCounterForce;
    float vertical;
    float rotationAxis;

#region runtime vars
    float verticalForce;
    Vector3 horizontalMove;

    public bool isGrounded;

    Quaternion physicalOrientation;

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
        horizontalMove = Vector3.zero;
        horizontalCounterForce = Vector3.zero;

        targetDirection = Vector3.zero;
        physicalOrientation = Quaternion.identity;
        //isGrounded = false;
    }

    public void SetYaw(float yaw)
    {
        this.yaw = yaw;
    }

    public void SetPitch(float pitch)
    {
        this.pitch = pitch;
    }

    public void SetRoll(float roll)
    {
        this.roll = roll;
    }

    public void SetPower(float power)
    {
        this.power = power;
    }

    public void SetHorizontalDirection(Vector3 targetDirection)
    {
        this.targetDirection = targetDirection;
    }

    public void SetVerticalInput(float verticalAxis)
    {
        this.vertical = verticalAxis;
    }

    public void SetRotation(float rotationAxis)
    {
        this.rotationAxis = rotationAxis;
    }

    // Update is called once per frame
    void Update()
    {
        // Autopilot : easy drive
        if (droneConfiguration.easyDrive && roll != 0)
        {
            pitch = (pitch + roll * droneConfiguration.rollToPitchRatio) / 2;
            yaw = roll * droneConfiguration.rollToYawRatio;
        }

        // GENERAL ENGINE
        Vector3 targetAngle = transform.localEulerAngles;
        // X axis : Pitch (tangage)
        targetAngle.x += pitch * Time.deltaTime * droneConfiguration.pitchSpeed;
        // Y axis : Yaw (lacet)
        targetAngle.y += yaw * Time.deltaTime * droneConfiguration.yawSpeed;
        // Z axis : Roll (roulis)
        targetAngle.z += roll * Time.deltaTime * droneConfiguration.rollSpeed;
        // Power
        verticalForce = Mathf.Lerp(verticalForce, power * droneConfiguration.maxPower, Time.deltaTime * droneConfiguration.powerSpeed);

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
        // set to limits if any
        transform.localEulerAngles = targetAngle;

        if (visualOrientationFactor != 1)
        {
            Vector3 visualEulerAngles = -targetAngle * visualOrientationFactor;
            // No change on yaw (Y) axis
            visualEulerAngles.y = 0;
            visualTransform.localEulerAngles = visualEulerAngles;
        }

        // AUTO PILOT

        // Auto stability force to have the local Y axis up, without changing the actual yaw (Y)
        if (roll == 0 && pitch == 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.eulerAngles.y, 0), droneConfiguration.horizontalAutoStability * Time.deltaTime);
        }

        // Maintain height if stable enough and no power is requested by user
        float angleToUp = Vector3.Angle(transform.up, Vector3.up);
        if (droneConfiguration.maintainHeight && power == 0 && angleToUp < maxAngleToLift)
        {
            float forceToMaintainHeight = massToLift * -Physics.gravity.y;
            forceToMaintainHeight -= body.velocity.y * verticalVelocityRatio;
            // Take into account angle between world Vector3.up and transform.up to increase vertical force to maintain height
            // Quaternion.Angle returns degrees but Mathf.Cos uses radians dafuck
            verticalForce = forceToMaintainHeight / Mathf.Cos(angleToUp * Mathf.Deg2Rad);
        }

        // Maintain position if no change is request by user (no pitch/yaw/roll)
        if (pitch == 0 && roll == 0 && yaw == 0)
        {
            // TODO : use latest known position instead of body velocity

            // TODO : oriente body to reflect that change (invert rotation)
           
        }

        // TODO : extract visual
        for (int i = 0; i < reactorPSList.Length; i++)
        {
            var emission = reactorPSList[i].emission;
            emission.rateOverTime = reactorEmissionCurve.Evaluate(body.velocity.sqrMagnitude / droneConfiguration.maxPower);
        }
    }

    void FixedUpdate()
    {
        if (verticalForce != 0)
        {
            body.AddForce(transform.up * verticalForce);
        }
        /*
        if (horizontalCounterForce != Vector3.zero)
        {
            body.AddForce(horizontalCounterForce);
        }
// */
        if (droneConfiguration.freeDragRatio != 0)
            body.velocity *= droneConfiguration.freeDragRatio;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * verticalForce);
    }
}
