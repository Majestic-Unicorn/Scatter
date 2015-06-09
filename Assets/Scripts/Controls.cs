using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {
	//public Transform camera;

	public float speed;

	void Update () {
		Vector3 input = new Vector3 (Input.GetAxis ("Horizontal") * speed, 0, Input.GetAxis ("Vertical") * speed);

		GetComponent<Rigidbody> ().AddForce (input);

		transform.LookAt (input);
	}
}
