using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunePillar : MonoBehaviour
{

    private bool _isActive = false;
    [SerializeField] private ParticleSystem particle;

    private PuzzleController controller;

    void Awake()
    {
        controller = GetComponentInParent<PuzzleController>();
    }

    public bool Active
    {
        get
        {
            return _isActive;
        }

        private set
        {
            _isActive = value;
        }
    }

    public void SetActive()
    {
        if (Active)
            return;
        Active = true;
        particle.Play();
        controller.TriggerPillar(this);
    }

    public void ResetActive()
    {
        if (!Active)
            return;
        Active = false;
        particle.Stop();
    }

}
