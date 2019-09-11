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

    public List<Spell> Spells;

    private float[] coolDowns;

    // Start is called before the first frame update
    void Start()
    {
        coolDowns = new float[Spells.Count];
        MagicPool.Instance.Initialize(Spells);
    }

    // Update is called once per frame
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
