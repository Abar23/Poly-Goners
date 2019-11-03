using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomController : MonoBehaviour
{

    public UnityEvent OnClearRoom;
    private List<ISkeleton> enemies;

    private int m_NumEnemy = 0;

    void Awake()
    {
        enemies = new List<ISkeleton>();
    }

    public void RegisterEnemy(ISkeleton enemy)
    {
        m_NumEnemy++;
        enemies.Add(enemy);
    }

    public void RemoveEnemy(ISkeleton enemy)
    {
        m_NumEnemy--;
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
        if (m_NumEnemy <= 0 && OnClearRoom != null)
        {
            OnClearRoom.Invoke();
        }
    }

    public void SpawnEnemy()
    {
        foreach (ISkeleton enemy in enemies)
        {
            enemy.Spawn();
        }
    }


}
