﻿using UnityEngine;
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

    private bool gotGem = false;

    private Transform gem;

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

    public bool able(){
        if (fallCoolDown != 0)
            return false;

        return true;
    }

    public void gemGet(Transform gem){
        animator.SetLayerWeight(1, 1);
        animator.SetLayerWeight(2, 1);

        this.gem = gem;

        gotGem = true;
    }

    public void Fall(){
        fallCoolDown = 2f;
        animator.SetBool("Fall", true);
        //GetComponent<CapsuleCollider>().enabled = false;

        dropGem();
    }

    public void dropGem(){
        if (gotGem){
            gotGem = false;
            gem.transform.parent = null;

            gem.GetComponent<Gem>().drop();

            gem = null;
        }
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
            input.Set(Input.GetAxis(string.Concat("Horizontal_", player)), 0, Input.GetAxis(string.Concat("Vertical_", player)));

            if (Input.GetAxis(string.Concat("Fire3_", player)) != 0)
                dBoost = boostSpeed;

            //if (Input.GetAxis(string.Concat("Fire2_", player)) != 0)
                //dBoost = boostSpeed;

            if (!gotGem){
                if ((Input.GetAxis(string.Concat("Jump_", player)) != 0 && !armsOut) || armsMid){
                    if (!armsMid){
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
        }

        //if (player == 2 && fallCoolDown == 0f)
        //    input.Set(0f, 0f, -.9f);

        if (!gotGem && fallCoolDown == 0){
            animator.SetLayerWeight(1, lArm);
            animator.SetLayerWeight(2, rArm);
        }
        else if (fallCoolDown != 0){
            animator.SetLayerWeight(1, 0);
            animator.SetLayerWeight(2, 0);
        }

        animator.SetFloat("Speed", input.magnitude);

        rigidBody.AddForce(input * (runSpeed + dBoost) * Time.deltaTime);

        transform.LookAt(transform.position + input.normalized);
	}
}