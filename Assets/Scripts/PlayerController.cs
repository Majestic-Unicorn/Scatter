using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class PlayerController : MonoBehaviour {
    private Rigidbody rigidBody;
    private Animator animator;

    public int player = 1;
    public float speed;

    void Start(){
        rigidBody = GetComponentInChildren<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

	void Update () {
  		Vector3 input = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));

        animator.SetFloat("Speed", input.magnitude);

        rigidBody.AddForce(input * speed * Time.deltaTime);

     	transform.LookAt (transform.position + input);
	}
}