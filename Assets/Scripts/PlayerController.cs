using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    private Rigidbody rigidBody;
    private Animator animator;

    public int player = 1;
    public float speed = 20000f;

    public float boost = 10000f;

    private float lArm = 0f;
    private float rArm = 0f;

    private bool armsOut = false;
    private bool armsMid = false;

    public float pushSpeed = 500f;

    void Start(){
        rigidBody = GetComponentInChildren<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

	void Update (){
        Vector3 input = new Vector3(0, 0, 0);
        float dBoost = 0f;

        if (player == 1){
            input.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (Input.GetKey(KeyCode.LeftShift))
                dBoost = boost;

            if ((Input.GetAxis("Jump") != 0 && !armsOut) || armsMid){
                if (!armsMid)
                    armsMid = true;

                lArm += ((1f - lArm) * .25f * pushSpeed) * Time.deltaTime;
                rArm += ((1f - rArm) * .25f * pushSpeed) * Time.deltaTime;

                if (lArm > 0.9f && rArm > 0.999f){
                    armsOut = true;
                    armsMid = false;
                }
            }
            else{
                lArm += ((0f - lArm) * .05f * pushSpeed) * Time.deltaTime;
                rArm += ((0f - rArm) * .05f * pushSpeed) * Time.deltaTime;

                if (lArm < 0.1f && rArm < 0.001f)
                    armsOut = false;
                else
                    armsOut = true;
            }
        }

        animator.SetLayerWeight(1, lArm);
        animator.SetLayerWeight(2, rArm);

        animator.SetFloat("Speed", input.magnitude);

        rigidBody.AddForce(input * (speed + dBoost) * Time.deltaTime);

        transform.LookAt(transform.position + input);
	}
}