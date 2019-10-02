using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MagicBox : MonoBehaviour
{

    [Serializable]
    public struct Spell
    {
        public GameObject Object;
        public float CoolDown;
        public int MagicPoint;
    }

    [SerializeField] private Alignment m_Alignment;

    [SerializeField] private List<Spell> m_Spells;

    [SerializeField] private int m_MagicPoint;

    private float[] coolDowns;
    private MagicPool pool;
    private int magicMax;

    void Awake()
    {
        coolDowns = new float[m_Spells.Count];
        pool = new MagicPool();
        foreach (Spell spell in m_Spells)
        {
            Damager damager = spell.Object.GetComponent<Damager>();
            if (damager != null)
            {
                damager.Alignment = m_Alignment;
            }
        }
        pool.Initialize(m_Spells);
        magicMax = m_MagicPoint;
    }

    void Update()
    {
        for (int i = 0; i < coolDowns.Length; i++)
        {
            coolDowns[i] = Mathf.Max(0, coolDowns[i] - Time.deltaTime);
        }
    }

    public bool FireMagic(int index)
    {
        if (coolDowns[index] > 0)
        {
            return false;
        }
        if (!CheckMagicPoint(m_Spells[index].MagicPoint))
        {
            return false;
        }
        GameObject magic = pool.Require(index);
        magic.transform.rotation = transform.rotation;
        magic.transform.position = transform.position;
        Projectile projectile = magic.GetComponent<Projectile>();
        if (projectile == null)
        {
            return false;
        }
        projectile.ProjectileInvoke();
        coolDowns[index] = m_Spells[index].CoolDown;
        ReduceMagicPoint(m_Spells[index].MagicPoint);
        return true;
    }
    
    public int GetNumberOfSpells()
    {
        return m_Spells.Count;
    }

    public void IncreaseMagicPoint(int number)
    {
        if (number < 0) return;
        m_MagicPoint = Mathf.Min(number, magicMax);
    }

    bool CheckMagicPoint(int number)
    {
        return m_MagicPoint - number >= 0;
    }

    void ReduceMagicPoint(int number)
    {
        m_MagicPoint -= number;
    }

    public float GetMagicPointRatio()
    {
        return m_MagicPoint / (float)magicMax;
    }

    public void RestoreFullMP()
    {
        m_MagicPoint = magicMax;
    }
}
