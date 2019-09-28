using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestBox : MonoBehaviour
{

    [SerializeField] private GameObject m_Lid;
    [SerializeField] private UnityEvent m_AfterOpen;
    [SerializeField] private Vector3 m_Axis;

    private const float k_openAngle = 60f;
    private const int k_numSteps = 50;
    private bool isOpen = false;

    public void OpenChest()
    {
        if (!isOpen)
        {
            StartCoroutine(OpenLid());
            isOpen = true;
        }
    }

    IEnumerator OpenLid()
    {
        float step_angle = k_openAngle / k_numSteps;
        for (int step = 0; step < k_numSteps; step++)
        {
            m_Lid.transform.Rotate(m_Axis, step_angle);
            yield return new WaitForEndOfFrame();
        }
        if (m_AfterOpen != null)
        {
            m_AfterOpen.Invoke();
        }
    }

}
