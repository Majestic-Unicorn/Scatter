using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchSet : MonoBehaviour {
    public HashSet<Transform> set = new HashSet<Transform>();

    public string searchTag = "Player";

    void OnTriggerEnter(Collider other){
        if (other.tag == searchTag)
            set.Add(other.transform);
    }

    void OnTriggerExit(Collider other){
        if (other.tag == searchTag)
            set.Remove(other.transform);
    }
}
