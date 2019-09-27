using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomController : MonoBehaviour
{

    public UnityEvent OnClearRoom;

    private int m_NumEnemy = 0;

    public void RegisterEnemy()
    {
        m_NumEnemy++;
    }

    public void RemoveEnemy()
    {
        m_NumEnemy--;
        if (m_NumEnemy <= 0 && OnClearRoom != null)
        {
            OnClearRoom.Invoke();
        }
    }


}
