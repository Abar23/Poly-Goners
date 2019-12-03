using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : MonoBehaviour
{
    private float rotateSpeed = 15f;

    void Start() {
        
    }

    void Update() {
        this.transform.Rotate(rotateSpeed, 0.0f, 0.0f, Space.Self);
    }
}
