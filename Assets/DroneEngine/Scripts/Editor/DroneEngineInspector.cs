using UnityEngine;
using UnityEditor;
using System.Collections;

namespace FredericRP.DroneEngine
{
    [CustomEditor(typeof(DroneController))]
    public class ArcadeCopterInspector : Editor
    {

        static float velocityArrowSize = 3;//0.05f;
        static float moveArrowSize = 10;//0.05f;

        void OnSceneGUI()
        {
            DroneController copter = (target as DroneController);
            //CharacterController cc = copter.GetComponent<CharacterController>();
            Transform transform = copter.transform;

            Handles.color = Color.red;// (copter.isGrounded ? Color.white : Color.red);
                                      /*
                                      Handles.color = Color.white;
                                      Quaternion ccQuat = new Quaternion();
                                      ccQuat.SetFromToRotation(Vector3.forward, cc.velocity);
                                      Handles.ArrowCap(0, transform.position, ccQuat, cc.velocity.sqrMagnitude*velocityArrowSize);
                                      // */
                                      //
            Rigidbody rigidbody = copter.GetComponent<Rigidbody>();
            Quaternion velocityQuat = new Quaternion();
            velocityQuat.SetFromToRotation(Vector3.forward, rigidbody.velocity);
            Handles.ArrowHandleCap(0, transform.position, velocityQuat, rigidbody.velocity.sqrMagnitude * velocityArrowSize, EventType.Ignore);
            // */

            Handles.color = Color.green;// (copter.isGrounded ? Color.cyan : Color.green);
            Quaternion moveQuat = new Quaternion();
            //moveQuat.SetFromToRotation(Vector3.forward, Vector3.up * (target as ArcadeCopter).verticalForce);
            //Handles.ArrowHandleCap(0, transform.position+Vector3.forward, moveQuat, Mathf.Abs((target as ArcadeCopter).verticalForce)*moveArrowSize, EventType.Ignore);

            Handles.color = (copter.isGrounded ? Color.black : Color.blue);
            Quaternion moveHQuat = new Quaternion();
            //moveHQuat.SetFromToRotation(Vector3.forward, (target as ArcadeCopter).horizontalMove);
            //Handles.ArrowHandleCap(0, transform.position+Vector3.forward, moveHQuat, (target as ArcadeCopter).horizontalMove.magnitude*moveArrowSize, EventType.Ignore);
        }

    }
}