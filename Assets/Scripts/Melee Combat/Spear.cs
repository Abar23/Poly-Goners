using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Weapon
{
    private Vector3 startPosition = new Vector3(-0.474f, 0.033f, 0.964f);
    private Vector3 startRotation = new Vector3(-71.232f, -119.827f, 89.33301f);
    //private Vector3 startRotation = new Vector3(36.411f, -125.179f, -252.613f);

    void Start()
    {
        this.transform.localPosition = startPosition;
        this.transform.localEulerAngles = startRotation;
        base.Start();
    }
}
