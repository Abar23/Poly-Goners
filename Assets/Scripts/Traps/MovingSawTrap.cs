using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSawTrap : MonoBehaviour
{
    private float rotateSpeed = 5f;
    public Transform leftEndpoint;
    public Transform rightEndpoint;
    private bool goingToRightEndpoint;
    private bool retarget;
    private Vector3 velocity;
    private float retargetDistance = .5f;
    private float previousDist = Mathf.Infinity;
    public float moveSpeed;
    private Rigidbody rb;
    public GameObject sawRail;
    private Vector3 railSize;
    //[Range(1.0f, 5.0f)]
    //public float trackSize;

    void Start() {
        rb = this.GetComponent<Rigidbody>();
        goingToRightEndpoint = true;
        retarget = false;
        TargetRightEndpoint();

        //sawRail.transform.localScale = new Vector3(sawRail.transform.localScale.localScale.x, sawRail.transform.localScale.localScale.y, trackSize);
        //sawRail.gameObject.transform.localScale = new Vector3(sawRail.gameObject.transform.localScale.x, sawRail.transform.localScale.y, trackSize);
        var renderer = sawRail.GetComponent<MeshRenderer>();
        railSize = renderer.bounds.size;
        //Debug.Log(railSize.z);
        leftEndpoint.localPosition = new Vector3(0, 0, this.transform.localPosition.z + -railSize.z / 2 + 1.25f);
        rightEndpoint.localPosition = new Vector3(0, 0, this.transform.localPosition.z + railSize.z / 2 - 1.25f);
    }

    void Update() {
        rotateSpeed = moveSpeed;
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
