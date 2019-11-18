using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleDungeonDoor : Door
{

    [SerializeField] private float m_OpenAngle;
    [SerializeField] private AudioSource m_OpenSFX;
    [SerializeField] private AudioSource m_CloseSFX;
    private const int k_RotationInterval = 30;

    public override IEnumerator OpenDoor()
    {
        m_OpenSFX.Play();
        for (int i = 0; i < k_RotationInterval; i++)
        {
            transform.Rotate(Vector3.up * m_OpenAngle / k_RotationInterval);
            yield return new WaitForFixedUpdate();
        }
        m_IsOpen = true;
        isMoving = false;
    }

    public override IEnumerator CloseDoor()
    {
        for (int i = 0; i < k_RotationInterval; i++)
        {
            transform.Rotate(Vector3.up * m_OpenAngle / -k_RotationInterval);
            yield return new WaitForFixedUpdate();
        }
        m_CloseSFX.Play();
        m_IsOpen = false;
        isMoving = false;
    }

}
