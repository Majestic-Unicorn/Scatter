﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class MultiCamera : MonoBehaviour {
    public Camera mainCamera;
    public Transform[] targets;

    public float relativeYaw = 0;
    public float relativePitch = 25;

    private float height = 100;

    public float zoomInZone = 1f;
    public float zoomOutZone = 1f;

    public float zoomSpeed = 1f;
    public float ease = 0.075f;

    void Start() {
        Vector3 average = AverageTarget();

        Vector3 movement = RelativeRotation(average);

        transform.position = movement;

        transform.LookAt(average);

        mainCamera = GetComponent<Camera>();
    }

    private Vector3 RelativeRotation(Vector3 axis) {
        axis = Quaternion.AngleAxis(relativeYaw, Vector3.up) * axis;

        Vector3 direction = new Vector3(0, height, 0);

        direction = Quaternion.Euler(-relativePitch, relativeYaw, 0) * direction;

        return direction + axis;
    }

    private Vector3 AverageTarget() {
        Vector3 min = new Vector3(0, 0, 0);
        Vector3 max = new Vector3(0, 0, 0);

        Vector3 average = new Vector3(0, 0, 0);

        for (int i = 0; i < targets.Length; i++){
            if (targets[i].position.x < min.x)
                min.x = targets[i].position.x;
            else if (targets[i].position.x > max.x)
                max.x = targets[i].position.x;

            if (targets[i].position.y < min.y)
                min.y = targets[i].position.y;
            else if (targets[i].position.y > max.y)
                max.y = targets[i].position.y;

            if (targets[i].position.z < min.z)
                min.z = targets[i].position.z;
            else if (targets[i].position.z > max.z)
                max.z = targets[i].position.z;
            
            average += targets[i].position;
        }

        return (max + min) / 2;
    }

    private bool inScreen(Vector3 target, float reduction) {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target);

        if (screenPos.x < 0 + mainCamera.pixelWidth * reduction)
            return false;

        if (screenPos.y < 0 + mainCamera.pixelHeight * reduction)
            return false;

        if (screenPos.x > mainCamera.pixelWidth - mainCamera.pixelWidth * reduction)
            return false;

        if (screenPos.y > mainCamera.pixelHeight - mainCamera.pixelHeight * reduction)
            return false;

        return true;
    }

	void FixedUpdate () {
        bool zoomOut = false;
        bool zoomIn = true;

        for (int i = 0; i < targets.Length; i++){
            if (!inScreen(targets[i].position, zoomOutZone))
                zoomOut = true;

            if (!inScreen(targets[i].position, zoomInZone))
                zoomIn = false;
        }

        if (zoomOut)
            height += zoomSpeed;

        if (zoomIn)
            height -= zoomSpeed;

        Vector3 average = AverageTarget();

        Vector3 newPos = RelativeRotation(average);    

        newPos = Vector3.Lerp(transform.position, newPos, ease);

        transform.position = newPos;

        transform.LookAt(average);
	}
}