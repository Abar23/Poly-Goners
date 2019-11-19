using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunePillar : MonoBehaviour
{

    private bool _isActive = false;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Material m_ActiveMaterial;
    private Material m_DefaultMaterial;

    private PuzzleController controller;

    void Awake()
    {
        controller = GetComponentInParent<PuzzleController>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            m_DefaultMaterial = meshRenderer.material;
        }
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
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material = m_ActiveMaterial;
        }
    }

    public void ResetActive()
    {
        if (!Active)
            return;
        Active = false;
        particle.Stop();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material = m_DefaultMaterial;
        }
    }

}
