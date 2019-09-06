using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple class to move in a circle. Could have required the object to be on the circle and then just rotate, 
//   but this allows the object to be centered at (0,y,0).
public class CirclePathXZ : MonoBehaviour
{
    public float radius = 3f;
    public float speed = 30f;
    private float theta = 0;
    const float MaxAngle = Mathf.PI * 100;

    // Update is called once per frame
    void Update()
    {
        theta += speed * Time.deltaTime;
        Vector3 localPos = transform.localPosition;
        localPos.x = radius * Mathf.Sin(theta);
        localPos.z = radius * Mathf.Cos(theta);
        transform.localPosition = localPos;
        // Reset every now and then for stability.
        if (theta > MaxAngle) theta -= MaxAngle;
    }
}
