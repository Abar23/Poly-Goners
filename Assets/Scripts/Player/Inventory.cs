using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<IWeapon> meleeWeapons;
    private List<Projectile> magicAbilities;

    void Start()
    {
        meleeWeapons = new List<IWeapon>();
        magicAbilities = new List<Projectile>();
    }
}
