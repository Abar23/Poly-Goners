using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ItemTimer : MonoBehaviour
{

    [Range(0, 1)]
    [SerializeField] private float m_BlinkInterval = 0.1f;
    [Range(5, 30)]
    [SerializeField] private float m_DisableTimer = 20f;

    private float k_BlinkTime = 5f;
    private MeshRenderer renderer;

    void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    IEnumerator Blink(float delay)
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(m_BlinkInterval);
        }
    }

    void DestroySelf()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(Blink(m_DisableTimer - k_BlinkTime));
        Invoke("DestroySelf", m_DisableTimer);
    }
}
