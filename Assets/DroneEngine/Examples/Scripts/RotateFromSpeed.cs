using UnityEngine;

namespace FredericRP.DroneEngine
{
    public class RotateFromSpeed : MonoBehaviour
    {
        [SerializeField]
        DroneController controller;
        [SerializeField]
        Transform[] rotorList;
        [SerializeField]
        Vector3 rotorAxis = Vector3.up;
        [SerializeField]
        float rotorSpeedRatio = 10;
        [SerializeField]
        float rotorSpeedOffset = 100;

        private void Start()
        {
            if (controller == null)
                controller = GetComponent<DroneController>();
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < rotorList.Length; i++)
            {
                rotorList[i].Rotate(rotorAxis, (controller.RotorSpeed + rotorSpeedOffset) * rotorSpeedRatio * Time.deltaTime);
            }
        }
    }
}