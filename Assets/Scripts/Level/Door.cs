using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] protected bool m_IsOpen = true;
     
    protected bool isMoving = false;

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
    public virtual IEnumerator OpenDoor()
    {
        throw new NotImplementedException();
    }

    public virtual IEnumerator CloseDoor()
    {
        throw new NotImplementedException();
    }
}
