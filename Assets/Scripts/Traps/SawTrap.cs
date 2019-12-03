using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationarySawTrap : MonoBehaviour
{
    private float rotateSpeed = 10f;

    void Update() {
        this.transform.Rotate(rotateSpeed, 0.0f, 0.0f, Space.Self);
    }
}
