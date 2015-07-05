using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
    public Transform[] targets;

    public float relativeYaw = 0;
    public float relativePitch = 25;

    public float height = 100;

    public float ease = 0.075f;

    void Start() {
        Vector3 average = AverageTarget();

        Vector3 movement = RelativeRotation(average);

        transform.position = movement;

        transform.LookAt(average);
    }

    Vector3 RelativeRotation(Vector3 axis) {
        axis = Quaternion.AngleAxis(relativeYaw, Vector3.up) * axis;

        Vector3 direction = new Vector3(0, height, 0);

        direction = Quaternion.Euler(-relativePitch, relativeYaw, 0) * direction;

        return direction + axis;
    }

    Vector3 AverageTarget() {
        Vector3 average = new Vector3(0, 0, 0);

        for (int i = 0; i < targets.Length; i++)
        {
            average += targets[i].position;
        }

        return average / targets.Length;
    }

	void FixedUpdate () {
        Vector3 average = AverageTarget();
        
        Vector3 movement = RelativeRotation(average);

        movement = Vector3.Lerp(transform.position, movement, ease);

        transform.position = movement;

        transform.LookAt(average);
	}
}