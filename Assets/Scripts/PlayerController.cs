using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {
	public float moveSpeed = 50;
	public float tiltAngle = 10;
	public float turnSpeed = 10;
	public float hoverHeight = 5;
	public float hoverForce = 50;

	private Vector2 _turnInput;
	private Rigidbody _rigidBody;

	void Start () {
		_rigidBody = GetComponent<Rigidbody> ();
	}
	
	void Update () {
		_turnInput = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
	}

	void FixedUpdate () {
		Ray ray = new Ray (transform.position, -transform.up);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, hoverHeight)) {
			float force = (hoverHeight - hit.distance) / hoverHeight;
			Vector3 hover = Vector3.up * force * hoverForce;

			_rigidBody.AddForce(hover);
		}

		_rigidBody.AddRelativeForce (0, 0, _turnInput.y * moveSpeed);

		_rigidBody.AddTorque (0, _turnInput.x * turnSpeed, 0);

		//_rigidBody.AddTorque (transform.rotation.z + _turnInput.y * tiltAngle, 0, transform.rotation.x + _turnInput.x * -tiltAngle);
	}
}