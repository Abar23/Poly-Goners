using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemHammer : Weapon
{
    private Vector3 startPosition = new Vector3(0.079f, -0.056f, -0.207f);
    private Vector3 startRotation = new Vector3(0f, 100f, -120f);


    void Start()
    {
        this.transform.localPosition = startPosition;
        this.transform.localEulerAngles = startRotation;
        base.Start();
    }
}