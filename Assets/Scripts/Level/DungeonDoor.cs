using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDoor : Door
{

    [SerializeField] private float m_OpenAngle;
    private const int k_RotationInterval = 30;

    public override IEnumerator OpenDoor()
    {
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
        m_IsOpen = false;
        isMoving = false;
    }

}
