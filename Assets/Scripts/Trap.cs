using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 public class Pair<T, U>{
     public T First { 
         get; private set; 
     }

     public U Second { 
         get; set; 
     }

     internal Pair(T first, U second){
         First = first;
         Second = second;
     }
 }

public class Trap : MonoBehaviour {
    public HashSet<Pair<Transform, float>> set = new HashSet<Pair<Transform, float>>();

    public float coolDown = 10;

    public bool recurring = true;

    public string searchTag = "Player";

    private Animator animator;

    private float primeTime = 0.5f;

    void Start(){
        animator = GetComponent<Animator>();

        //animator.Play("Prime");
    }

    void Update(){
        primeTime -= Time.deltaTime;

        if (primeTime < 0)
            primeTime = 0;

        foreach (Pair<Transform, float> pair in set){
            pair.Second -= Time.deltaTime;

            if (pair.Second < 0){
                pair.Second = coolDown;

                PlayerController player = pair.First.GetComponent<PlayerController>();

                if (player && recurring && primeTime == 0){
                    player.Fall();
                    animator.Play("Fire");
                }
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == searchTag){
            set.Add(new Pair<Transform, float>(other.transform, coolDown));

            PlayerController player = other.GetComponent<PlayerController>();

            if (player && primeTime == 0){
                player.Fall();

                player.transform.Rotate(new Vector3(0, 180, 0));
                animator.Play("Fire");
            }
        }
    }

    void OnTriggerExit(Collider other){
        foreach (Pair<Transform, float> pair in set){
            if (pair.First == other.transform){
                set.Remove(pair);
                break;
            }
        }
    }
}
