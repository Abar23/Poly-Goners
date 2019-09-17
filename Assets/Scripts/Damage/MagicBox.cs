using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBox : MonoBehaviour
{

    [Serializable]
    public struct Spell
    {
        public GameObject Object;
        public float CoolDown;
    }

    [SerializeField] private Alignment m_Alignment;

    [SerializeField] private List<Spell> m_Spells;

    private float[] coolDowns;
    private MagicPool pool;

    void Awake()
    {
        coolDowns = new float[m_Spells.Count];
        pool = new MagicPool();
        pool.Initialize(m_Spells);
        foreach (Spell spell in m_Spells)
        {
            Damager damager = spell.Object.GetComponent<Damager>();
            if (damager != null)
            {
                damager.Alignment = m_Alignment;
            }
        }
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
        GameObject magic = pool.Require(index);
        Projectile projectile = magic.GetComponent<Projectile>();
        if (projectile == null)
        {
            return false;
        }
        projectile.ProjectileInvoke();
        coolDowns[index] = m_Spells[index].CoolDown;
        return true;
    }
}
