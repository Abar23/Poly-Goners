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
        public bool IsConsistent;
    }

    [SerializeField] private Alignment m_Alignment;

    [SerializeField] private List<Spell> m_Spells;

    [SerializeField] private int m_MagicPoint;

    private float[] coolDowns;
    private MagicPool pool;
    private int magicMax;
    private bool _drainMana = false;

    public Dictionary<string, int> magicAbilites { get; private set; }


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

        magicAbilites = new Dictionary<string, int>
        {
            { "Lightning Ball Pickup", 0 },
            { "Burning Fire Ball Pickup", 1 },
            { "Fast Fire Ball Pickup", 2 },
            { "Explosive Fire Ball Pickup", 3 },
            { "Poisonous Ball Pickup", 4 },
            { "Fire Pulse Pickup", 5 },
            { "Myst Pulse Pickup", 6 },
            { "Poisonous Pulse Pickup", 7 },
            { "Snow Pulse Pickup", 8 },
        };
    }

    void Update()
    {
        for (int i = 0; i < coolDowns.Length; i++)
        {
            coolDowns[i] = Mathf.Max(0, coolDowns[i] - Time.deltaTime);
        }
    }

    public int GetIndexFromName(string name)
    {
        return magicAbilites[name];
    }

    public bool CheckMagic(int index)
    {
        if (coolDowns[index] > 0)
        {
            return false;
        }
        if (!CheckMagicPoint(m_Spells[index].MagicPoint))
        {
            return false;
        }
        return true;
    }

    public bool IsPulseMagic(int index)
    {
        return m_Spells[index].IsConsistent;
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
        if (m_Spells[index].IsConsistent)
        {
            if (m_Spells[index].Object.activeSelf == false)
            {
                m_Spells[index].Object.SetActive(true);
            }
            m_Spells[index].Object.GetComponent<ParticleSystem>().Play();
            _drainMana = true;
            StartCoroutine(DrainMana(index));
        }
        else
        {
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
        }
        return true;
    }

    private IEnumerator DrainMana(int index)
    {
        while (_drainMana)
        {
            ReduceMagicPoint(1);
            if (!CheckMagicPoint(1))
            {
                StopMagic(index);
                break;
            }
            yield return new WaitForSeconds(1f / m_Spells[index].MagicPoint);
        }
    }

    public bool StopMagic(int index)
    {
        if (!m_Spells[index].IsConsistent)
            return false;
        if (!m_Spells[index].Object.GetComponent<ParticleSystem>().IsAlive())
            return false;
        _drainMana = false;
        m_Spells[index].Object.GetComponent<ParticleSystem>().Stop();
        coolDowns[index] = m_Spells[index].CoolDown;
        PulseMagic pulseMagic = m_Spells[index].Object.GetComponent<PulseMagic>();
        if (pulseMagic != null)
        {
            pulseMagic.ResetCollider();
        }
        return true;
    }

    public int GetNumberOfSpells()
    {
        return m_Spells.Count;
    }

    public void IncreaseMagicPoint(int number)
    {
        if (number < 0) return;
        m_MagicPoint = Mathf.Min(m_MagicPoint + number, magicMax);
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

    public void ResetMagicToFull()
    {
        m_MagicPoint = magicMax;
    }

    public bool IsConsistent(string name)
    {
        if (name == null)
            return false;
        if (!magicAbilites.ContainsKey(name))
            return false;
        return m_Spells[magicAbilites[name]].IsConsistent;
    }
}
