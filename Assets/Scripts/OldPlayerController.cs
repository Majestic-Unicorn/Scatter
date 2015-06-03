using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class OldPlayerController : MonoBehaviour {
	private Vector3 _input;
	private Quaternion _rotation;
	//private Quaternion _tilt;
	
	public float maxSpeed;
	public float maxTilt;
	public float maxTurn;
	
	public float ease;
	
	void Start () {
		_rotation = transform.rotation;
		
		maxSpeed = 50;
		maxTilt = 10;
		maxTurn = 10;
		ease = 1;
	}
	
	void Update () {
		_input = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		
		float x = _rotation.z + _input.z * maxTilt;
		float y = _rotation.y + _input.x * maxTurn;
		float z = _rotation.x + _input.x * -maxTilt;
		
		_rotation = Quaternion.Euler (x, y, z);
		
		transform.rotation = _rotation;
		
		//transform.Rotate (new Vector3 (0, _input.x * maxTurn, 0));
		
		Vector3 force = new Vector3 (0, 0, _input.z);
		
		GetComponent<Rigidbody> ().AddRelativeForce (force * maxSpeed);
		
		
	}
}