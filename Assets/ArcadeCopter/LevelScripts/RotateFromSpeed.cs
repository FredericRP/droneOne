using UnityEngine;

public class RotateFromSpeed : MonoBehaviour
{
    [SerializeField]
    Transform[] rotorList;
    [SerializeField]
    Vector3 rotorAxis = Vector3.up;
    [SerializeField]
    float rotorSpeedRatio = 10;
    [SerializeField]
    float rotorSpeedOffset = 100;
    ArcadeCopter copter;

    private void Start()
    {
        copter = GetComponent<ArcadeCopter>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < rotorList.Length; i++)
        {
            rotorList[i].Rotate(rotorAxis, (copter.RotorSpeed + rotorSpeedOffset) * rotorSpeedRatio * Time.deltaTime);
        }
    }
}
