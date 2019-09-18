using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPool
{

    public MagicPool()
    {

    }

    List<GameObject> samples;
    Dictionary<GameObject, List<GameObject>> pools;

    public void Initialize(List<MagicBox.Spell> spells)
    {
        samples = new List<GameObject>();
        foreach (MagicBox.Spell spell in spells)
        {
            samples.Add(spell.Object);
        }
        pools = new Dictionary<GameObject, List<GameObject>>();
        foreach (GameObject sample in samples)
        {
            pools.Add(sample, new List<GameObject>());
        }
    }

    public GameObject Require(GameObject sample)
    {
        GameObject result;
        if (pools[sample].Count <= 0)
        {
            GameObject newObject = MonoBehaviour.Instantiate(sample);
            Projectile projectile = newObject.GetComponent<Projectile>();
            projectile.RegistMagicPool(this);
            projectile.Sample = sample;
            pools[sample].Add(newObject);
        }
        result = pools[sample][0];
        pools[sample].RemoveAt(0);
        return result;
    }

    public GameObject Require(int index)
    {
        GameObject sample = samples[index];
        return Require(sample);
    }

    public bool Realse(GameObject magic)
    {
        Projectile projectile = magic.GetComponent<Projectile>();
        if (projectile == null)
        {
            return false;
        }
        pools[projectile.Sample].Add(magic);
        return true;
    }

}
