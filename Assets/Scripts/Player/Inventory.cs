using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int NumberOfMeleeSlots;
    public int NumberOfMagicSlots;
    public Text CoinDisplay; 

    private IWeapon[] meleeWeapons;
    private Projectile[] magicAbilities;
    private int gold;

    private int currentMeleeIndex;
    private int currentMagicIndex;
    
    void Start()
    {
        meleeWeapons = new IWeapon[NumberOfMeleeSlots];
        magicAbilities = new Projectile[NumberOfMagicSlots];
        currentMeleeIndex = 0;
        currentMagicIndex = 0;
    }

    public void IncreaseGold(int num)
    {
        gold += num;
        CoinDisplay.text = gold.ToString();
    }

    public void NextMeleeWeapon()
    {

    }

    public void NextMagicWeapon()
    {

    }
}
