using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	//public Transform camera;

	public float speed;

    void Start(){
        if (GetComponent<NetworkView>())
            if (!GetComponent<NetworkView>().isMine){
                enabled = false;
            }
            else{
                Follow camera = GameObject.FindGameObjectWithTag("Camera").GetComponent<Follow>();

                camera.transform.position = transform.position;
                camera.target = transform;
            }
    }

	void Update () {
  		Vector3 input = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));

        GetComponent<Rigidbody>().AddForce(input.normalized * speed * Time.deltaTime);

     	transform.LookAt (transform.position + input);
	}
}