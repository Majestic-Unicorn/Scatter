using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    [Range(1, 4)]
    public int player = 1;

    public float runSpeed = 20000f;
    public float boostSpeed = 10000f;
    public float pushSpeed = 500f;

    private Rigidbody rigidBody;
    private Animator animator;
    private TouchSet touchSet;

    private float lArm = 0f;
    private float rArm = 0f;

    private bool armsOut = false;
    private bool armsMid = false;

    private float fallCoolDown = 0f;

    void Start(){
        rigidBody = GetComponentInChildren<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        touchSet = GetComponentInChildren<TouchSet>();
    }

    void PushOthers(){
        foreach (Transform other in touchSet.set){
            PlayerController controller = other.GetComponent<PlayerController>();

            if (!controller.Fallen()){
                other.GetComponentInChildren<Animator>().SetBool("Fall", true);
                controller.Fall();
                other.transform.LookAt(transform);
            }
        }
    }

    bool Fallen(){
        if (fallCoolDown != 0f)
            return true;

        return false;
    }

    void Fall(){
        fallCoolDown = 2f;
        animator.SetBool("Fall", true);
        //GetComponent<CapsuleCollider>().enabled = false;
    }

    void GetUp(){
        fallCoolDown = 0f;
        //GetComponent<CapsuleCollider>().enabled = true;
    }

	void Update (){
        if (fallCoolDown != 0f){
            fallCoolDown -= Time.deltaTime;

            if (fallCoolDown < 1f)
                animator.SetBool("Fall", false);

            if (fallCoolDown < 0f || fallCoolDown == 0f){
                GetUp();
            }
        }

        Vector3 input = new Vector3(0, 0, 0);
        float dBoost = 0f;

        // The whole "player == 1" thing will be replaced with multi-controller support eventually

        if (player == 1 && fallCoolDown == 0f){
            input.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (Input.GetAxis("Fire3") != 0)
                dBoost = boostSpeed;

            if ((Input.GetAxis("Jump") != 0 && !armsOut) || armsMid){
                if (!armsMid)
                {
                    armsMid = true;
                    PushOthers();
                }

                lArm += ((1f - lArm) * .15f * pushSpeed) * Time.deltaTime;
                rArm += ((1f - rArm) * .15f * pushSpeed) * Time.deltaTime;

                if (lArm > 0.9f && rArm > 0.98f){
                    armsOut = true;
                    armsMid = false;
                }
            }
            else{
                lArm += ((0f - lArm) * .05f * pushSpeed) * Time.deltaTime;
                rArm += ((0f - rArm) * .05f * pushSpeed) * Time.deltaTime;

                if (lArm < 0.1f && rArm < 0.005f)
                    armsOut = false;
                else
                    armsOut = true;
            }
        }

        //if (player == 2 && fallCoolDown == 0f)
        //    input.Set(0f, 0f, -.9f);

        animator.SetLayerWeight(1, lArm);
        animator.SetLayerWeight(2, rArm);

        animator.SetFloat("Speed", input.magnitude);

        rigidBody.AddForce(input * (runSpeed + dBoost) * Time.deltaTime);

        transform.LookAt(transform.position + input.normalized);
	}
}