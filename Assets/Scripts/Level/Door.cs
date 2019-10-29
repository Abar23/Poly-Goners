using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]private bool m_IsOpen = false;
    [SerializeField] private float m_OpenAngle;
    private const int k_RotationInterval = 30;
    private bool isMoving = false;

    public void Open()
    {
        if (!m_IsOpen && !isMoving)
        {
            isMoving = true;
            StartCoroutine(OpenDoor());
        }
    }

    public void Close()
    {
        if (m_IsOpen && !isMoving)
        {
            isMoving = true;
            StartCoroutine(CloseDoor());
        }
    }
    IEnumerator OpenDoor()
    {
        for (int i = 0; i < k_RotationInterval; i++)
        {
            transform.Rotate(Vector3.up * m_OpenAngle / k_RotationInterval);
            yield return new WaitForFixedUpdate();
        }
        m_IsOpen = true;
        isMoving = false;
    }

    IEnumerator CloseDoor()
    {
        for (int i = 0; i < k_RotationInterval; i++)
        {
            transform.Rotate(Vector3.up * m_OpenAngle / -k_RotationInterval);
            yield return new WaitForFixedUpdate();
        }
        m_IsOpen = false;
        isMoving = false;
    }
}
