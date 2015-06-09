using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class Hover : MonoBehaviour {
	public float hoverHeight = 5;
	public float hoverForce = 50;
	
	private Rigidbody _rigidBody;

	void Start () {
		_rigidBody = GetComponent<Rigidbody> ();
	}
	
	void FixedUpdate () {
		Ray ray = new Ray (transform.position, -transform.up);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, hoverHeight)) {
			float force = (hoverHeight - hit.distance) / hoverHeight;
			Vector3 hover = Vector3.up * force * hoverForce;

			_rigidBody.AddForce(hover);
		}
	}
}