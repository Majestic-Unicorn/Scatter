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

    private float primeTime = 0.05f;

    private AudioSource audioPrime;
    private AudioSource audioTrigger;

    private bool triggered = false;

    void Start(){
        animator = GetComponent<Animator>();
        
        audioPrime = GetComponents<AudioSource>()[0];
        audioTrigger = GetComponents<AudioSource>()[1];

        audioPrime.Play();

        Destroy(transform.gameObject, 30);
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

                    audioTrigger.Play();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == searchTag && !triggered){
            set.Add(new Pair<Transform, float>(other.transform, coolDown));

            PlayerController player = other.GetComponent<PlayerController>();

            if (player && primeTime == 0){
                player.Fall();

                player.transform.Rotate(new Vector3(0, 180, 0));
                animator.Play("Fire");

                audioTrigger.Play();

                triggered = true;

                Destroy(transform.gameObject, 1);
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
