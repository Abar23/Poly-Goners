using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float Speed;

    public GameObject AfterEffect;

    private bool MotionActive = true;

    void FixedUpdate()
    {
        if (MotionActive)
            transform.Translate(Vector3.right * Speed * Time.deltaTime, Space.World);
    }

    public void Freeze()
    {
        MotionActive = false;
    }

    public void ScheduleRecap(float delay)
    {
        StartCoroutine("Recap", delay);
    }

    IEnumerator Recap(float delay)
    {
        yield return new WaitForSeconds(delay);
        AfterEffect.SetActive(false);
    }

}
