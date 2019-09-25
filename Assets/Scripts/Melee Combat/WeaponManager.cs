using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private GameObject sword;

    void Start()
    {
        // weapon indices must correspond to child number for GetChild
        sword = this.gameObject.transform.GetChild(0).gameObject;
    }

    public void EquipSword()
    {
        sword.SetActive(true);
    }

    public void UnequipSword()
    {
        sword.SetActive(false);
    }
}
