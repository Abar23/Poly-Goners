using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private GameObject sword;
    private GameObject axe;

    void Start()
    {
        // weapon indices must correspond to child number for GetChild (i.e. sword is the first child so use index 0)
        sword = this.gameObject.transform.GetChild(0).gameObject;
        axe = this.gameObject.transform.GetChild(1).gameObject;
    }

    public void EquipSword()
    {
        sword.SetActive(true);

        UnequipAxe(); // change this
    }

    public void UnequipSword()
    {
        sword.SetActive(false);
    }

    public void EquipAxe()
    {
        axe.SetActive(true);
                
        UnequipSword(); // change this
    }

    public void UnequipAxe()
    {
        axe.SetActive(false);
    }
}
