using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBox : MonoBehaviour
{

    [SerializeField] private GameObject m_Lid;

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
            m_Lid.transform.Rotate(-Vector3.right, step_angle);
            yield return new WaitForEndOfFrame();
        }
    }

}
