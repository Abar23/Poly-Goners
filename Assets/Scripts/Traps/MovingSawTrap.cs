using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSawTrap : MonoBehaviour
{
    private float rotateSpeed = 10f;
    public Transform leftEndpoint;
    public Transform rightEndpoint;
    private bool goingToRightEndpoint;
    private bool retarget;
    private Vector3 velocity;
    private float retargetDistance = .5f;
    private float previousDist = Mathf.Infinity;
    public float moveSpeed;
    private Rigidbody rb;
    private Vector3 startPosition;

    void Start() {
        rb = this.GetComponent<Rigidbody>();
        goingToRightEndpoint = true;
        retarget = false;
        TargetRightEndpoint();
        startPosition = this.transform.position;
    }

    void Update() {
        transform.position += velocity * moveSpeed * Time.deltaTime;

        if (retarget) {
            previousDist = Mathf.Infinity;
            if (goingToRightEndpoint) {
                TargetLeftEndpoint();
                goingToRightEndpoint = false;
                retarget = false;
            } else {
                TargetRightEndpoint();
                goingToRightEndpoint = true;
                retarget = false;
            }
        }

        if (goingToRightEndpoint) {
            this.transform.Rotate(rotateSpeed, 0.0f, 0.0f, Space.Self);
            float dist = Vector3.Distance(rightEndpoint.position, transform.position);
            if (dist <= retargetDistance || previousDist < dist) {
                retarget = true;
            }
            previousDist = dist;
        } else {
            this.transform.Rotate(-rotateSpeed, 0.0f, 0.0f, Space.Self);
            float dist = Vector3.Distance(leftEndpoint.position, transform.position);
            if (dist <= retargetDistance || previousDist < dist) {
                retarget = true;
                
            }
            previousDist = dist;
        }
    }

    private void TargetRightEndpoint() {
        velocity = rightEndpoint.transform.position - this.transform.position;
        velocity = Vector3.Normalize(velocity);
    }

    private void TargetLeftEndpoint() {
        velocity = leftEndpoint.transform.position - this.transform.position;
        velocity = Vector3.Normalize(velocity);
    }
}
