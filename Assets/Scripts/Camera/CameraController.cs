﻿using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;

    public float cameraZOffset;

    // Start is called before the first frame update
    void Start()
    {
        this.TrackTarget();
    }

    // Update is called once per frame
    void Update()
    {
        this.TrackTarget();
    }

    private void TrackTarget()
    {
        Vector3 targetPosition = this.target.transform.position;
        this.transform.position = new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z + cameraZOffset);
    }
}
