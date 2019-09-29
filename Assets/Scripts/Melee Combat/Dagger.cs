using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{
    private Vector3 startPosition = new Vector3(0.058f, 0.037f, 0.173f);
    private Vector3 startRotation = new Vector3(-71.232f, -119.827f, 89.33301f);

    void Start()
    {
        this.transform.localPosition = startPosition;
        this.transform.localEulerAngles = startRotation;
        base.Start();
    }
}
