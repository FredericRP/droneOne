using UnityEngine;
using System.Collections;
 
public class DontGoThroughThings : MonoBehaviour
{
	public LayerMask layerMask; //make sure we aren't in this layer 
	public float skinWidth = 0.1f; //probably doesn't need to be changed 
 
	private float minimumExtent;
	private float partialExtent;
	private float sqrMinimumExtent;
	private Vector3 previousPosition;
	public Transform objectRoot; 
	public Collider objectCollider;
	private Vector3 offset;
 
	//initialize values 
	void Awake() {
		GetComponent<Rigidbody>().useGravity = true;
		InitPosition();
	}
	
	void Start()
	{ 
		Vector3 extents = Vector3.Scale(transform.lossyScale, objectCollider.bounds.extents);
		minimumExtent = Mathf.Min (Mathf.Min (extents.x, extents.y), extents.z);
		partialExtent = minimumExtent * (1.0f - skinWidth);
		sqrMinimumExtent = minimumExtent * minimumExtent;
	}
	
	public void InitPosition() {
		previousPosition = objectCollider.bounds.center;
		offset = (objectCollider.bounds.center - objectRoot.transform.position);
	}
 
	void FixedUpdate ()
	{
		//have we moved more than our minimum extent? 
		Vector3 movementThisStep = objectCollider.bounds.center - previousPosition; 
		float movementSqrMagnitude = movementThisStep.sqrMagnitude;
 
		if (movementSqrMagnitude > sqrMinimumExtent) { 
			float movementMagnitude = Mathf.Sqrt (movementSqrMagnitude);
			RaycastHit hitInfo; 
 
			//check for obstructions we might have missed
			Debug.DrawLine(previousPosition + Vector3.left*0.1f, objectCollider.bounds.center + Vector3.left*0.1f, Color.green, 2);
			Debug.DrawRay (previousPosition + Vector3.right*0.1f, movementThisStep * movementMagnitude, Color.red, 2);
			if (Physics.Raycast (previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))  {
				Debug.DrawLine (previousPosition, hitInfo.point-offset, Color.yellow, 2);
				//ELog.Trace ("Hit " + hitInfo.transform.name + " move by (" + movementThisStep.magnitude + " vs " + movementMagnitude + ") * " + partialExtent + "=" + (movementThisStep.y / movementMagnitude) * partialExtent, gameObject);
				//ELog.Trace("Hit : position is " + objectRoot.position);
				objectRoot.position = (hitInfo.point - offset) - (movementThisStep / movementMagnitude) * partialExtent;
				//ELog.Trace("+Hit : replaced position is " + objectRoot.position);
			}
			previousPosition = objectCollider.bounds.center;
		}
 
	}
	/*
	void LateUpdate() {
		if (Time.timeScale > 0)
			ELog.Trace("Late Update : position is " + objectRoot.position);
	}
	// */
}