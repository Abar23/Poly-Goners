using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class PulseMagic : MonoBehaviour
{
    [SerializeField] private float m_MaxLength = 8f;
    [SerializeField] private float m_MaxTime = 1.5f;
    private CapsuleCollider capsuleCollider;
    private Coroutine extendCoroutine;

    void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public void ExtendCollider()
    {
        if (extendCoroutine != null)
        {
            extendCoroutine = StartCoroutine(UpdateCollider());
        }
    }

    public void ResetCollider()
    {
        StopCoroutine(extendCoroutine);
        capsuleCollider.height = 0;
        capsuleCollider.center = new Vector3(capsuleCollider.center.x, capsuleCollider.center.y, 0);
    }

    IEnumerator UpdateCollider()
    {
        float elapse = 0;
        while (elapse < m_MaxTime)
        {
            float length = m_MaxLength * elapse / m_MaxTime;
            capsuleCollider.height = length;
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, capsuleCollider.center.y, length / 2);
            yield return new WaitForFixedUpdate();
            elapse += Time.deltaTime;
        }
        capsuleCollider.height = m_MaxLength;
        capsuleCollider.center = new Vector3(capsuleCollider.center.x, capsuleCollider.center.y, m_MaxLength / 2);
    }
}
