using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomController : MonoBehaviour
{

    public UnityEvent OnClearRoom;
    private List<IEnemy> enemies;

    private int m_NumEnemy = 0;

    void Awake()
    {
        enemies = new List<IEnemy>();
    }

    public void RegisterEnemy(IEnemy enemy)
    {
        m_NumEnemy++;
        enemies.Add(enemy);
    }

    public void RemoveEnemy(IEnemy enemy)
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
        foreach (IEnemy enemy in enemies)
        {
            enemy.Spawn();
        }
    }


}
