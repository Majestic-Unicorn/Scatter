using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
	public Transform target = null;

	public Vector3 position;

    public float ease = 0.075f;

	void FixedUpdate () {
        if (target){
            Vector3 movement = target.position + position;

            movement = Vector3.Lerp(transform.position, movement, ease);

            transform.position = movement;

            transform.LookAt(target);
        }
	}
}