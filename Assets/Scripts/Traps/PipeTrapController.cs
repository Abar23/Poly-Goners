using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeTrapController : MonoBehaviour
{
    [Range(0.25f, 5.0f)]
    public float onTime;
    [Range(0.25f, 5.0f)]
    public float offTime;

    public float waitTime = 0.0f;

    public bool alwaysOn;

    private ParticleSystem particleSys;

    private void Start()
    {
        this.particleSys = this.GetComponentInChildren<ParticleSystem>();
        this.particleSys.Stop();

        // Just in case, turn off prewarm to make trap look like it is starting up
        ParticleSystem.MainModule main = this.particleSys.main;
        main.prewarm = false;

        if(!this.alwaysOn)
        {
            StartCoroutine(Wait());
        }
        else
        {
            this.particleSys.Play();
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(this.waitTime);
        StartCoroutine(TurnOnTrap());
    }

    IEnumerator TurnOnTrap()
    {
        this.particleSys.Play();
        yield return new WaitForSeconds(this.onTime);
        StartCoroutine(TurnOffTrap());
    }

    IEnumerator TurnOffTrap()
    {
        this.particleSys.Stop();
        yield return new WaitForSeconds(this.offTime);
        StartCoroutine(TurnOnTrap());
    }
}
