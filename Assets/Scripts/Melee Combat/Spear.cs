using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Weapon
{
    private Vector3 startPosition = new Vector3(-0.185f, 0.092f, 0.628f);
    private Vector3 startRotation = new Vector3(-115f, 60f, -80f);
    //private Vector3 startRotation = new Vector3(36.411f, -125.179f, -252.613f);

    void Start()
    {
        this.transform.localPosition = startPosition;
        this.transform.localEulerAngles = startRotation;
        base.Start();
    }
}
