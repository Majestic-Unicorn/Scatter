using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
	public Transform target;

	public Vector3 position;

	void FixedUpdate () {
		Vector3 movement = target.position + position;

		movement = Vector3.Lerp (transform.position, movement, 0.075f);

		transform.position = movement;

		transform.LookAt (target);
	}
}
