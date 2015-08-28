using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchSet : MonoBehaviour {
    public HashSet<Transform> set = new HashSet<Transform>();

    public string searchTag = "Player";

    void Update(){

        List<Transform> toRemove = new List<Transform>();

        int i = 0;
        foreach (Transform other in set){
            if (other.GetComponent<Collider>().enabled == false)
                toRemove.Insert(i, other);
        }

        foreach (Transform other in toRemove){
            set.Remove(other.transform);
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == searchTag)
            set.Add(other.transform);
    }

    void OnTriggerExit(Collider other){
        if (other.tag == searchTag)
            set.Remove(other.transform);
    }
}
