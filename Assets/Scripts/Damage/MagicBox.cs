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

    public List<Spell> Spells;

    private float[] coolDowns;

    void Awake()
    {
        coolDowns = new float[Spells.Count];
        MagicPool.Instance.Initialize(Spells);
        foreach (Spell spell in Spells)
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
        GameObject magic = MagicPool.Instance.Require(index);
        Projectile projectile = magic.GetComponent<Projectile>();
        if (projectile == null)
        {
            return false;
        }
        projectile.ProjectileInvoke();
        coolDowns[index] = Spells[index].CoolDown;
        return true;
    }
}
