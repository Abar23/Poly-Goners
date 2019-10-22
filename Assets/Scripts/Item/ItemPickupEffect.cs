using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupEffect : MonoBehaviour
{
    float rotationSpeed = 0.3f;
    float moveSpeed = 1f;
    float amplitude = .05f;
    Vector3 startPosition;

    void OnEnable()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        //transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + rotationSpeed, transform.eulerAngles.z);

        Vector3 newPosition = startPosition;
        newPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI * moveSpeed) * amplitude;
        transform.position = newPosition;
    }
}
