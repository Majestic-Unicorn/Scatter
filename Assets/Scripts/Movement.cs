using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	//public Transform camera;

	public float speed;

    void Start(){
        NetworkView network = GetComponent<NetworkView>();

        if (network)
            if (!network.isMine){
                enabled = false;
                return;
            }

        GameObject obj = GameObject.FindGameObjectWithTag("Camera");

        if (obj){
            //Follow camera = obj.GetComponent<Follow>();

            obj.transform.position = transform.position;
            //camera.target = transform;
        }
    }

	void Update () {
  		Vector3 input = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));

        GetComponent<Rigidbody>().AddForce(input.normalized * speed * Time.deltaTime);

     	transform.LookAt (transform.position + input);
	}
}