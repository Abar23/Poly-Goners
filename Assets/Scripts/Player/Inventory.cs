using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int NumberOfMeleeSlots;
    public int NumberOfMagicSlots;
    public Text CoinDisplay;
    public IconManager MeleeIcon;
    public List<GameObject> WeaponPrefabs;

    private Player player;
    private WeaponManager weaponManager;
    private Weapon[] meleeWeapons;
    private Projectile[] magicAbilities;
    private int gold;

    private int currentMeleeIndex;
    private int currentMagicIndex;
    
    void Start()
    {
        player = GetComponent<Player>();
        weaponManager = GetComponentInChildren<WeaponManager>();
        meleeWeapons = new Weapon[NumberOfMeleeSlots];
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
        currentMeleeIndex++;
        if (currentMeleeIndex == NumberOfMeleeSlots)
            currentMeleeIndex = 0;

        MeleeIcon.DisableCurrentIcon();

        if (meleeWeapons[currentMeleeIndex] == null)
        {
            weaponManager.UnequipCurrentWeapon();
        }
        else
        {
            weaponManager.EquipWeapon(meleeWeapons[currentMeleeIndex].gameObject.name + " Pickup");
            MeleeIcon.EnableIcon(meleeWeapons[currentMeleeIndex].gameObject.name);
        }

        player.ChangeCurrentWeapon(meleeWeapons[currentMeleeIndex]);
    }

    public void NextMagicWeapon()
    {

    }

    public void AddMeleeWeapon(Weapon weapon)
    {
        bool added = false;
        for (int i = 0; i < NumberOfMeleeSlots && !added; i++)
        {
            if (meleeWeapons[i] == null)
            {
                MeleeIcon.DisableCurrentIcon();
                meleeWeapons[i] = weapon;
                currentMeleeIndex = i;
                MeleeIcon.EnableIcon(meleeWeapons[currentMeleeIndex].gameObject.name);
                player.ChangeCurrentWeapon(meleeWeapons[currentMeleeIndex]);
                added = true;
            }
        }
    }

    public void DropWeapon()
    {
        weaponManager.UnequipCurrentWeapon();
        MeleeIcon.DisableCurrentIcon();
        foreach (GameObject w in WeaponPrefabs)
        {
            if (meleeWeapons[currentMeleeIndex].gameObject.name + " Pickup" == w.name)
            {
                GameObject item = Instantiate(w, transform.position + transform.forward, Quaternion.identity);
                item.name = w.name;
            }
        }

        meleeWeapons[currentMeleeIndex] = null;
        player.ChangeCurrentWeapon(meleeWeapons[currentMeleeIndex]);
    }

    public bool IsMeleeFull()
    {
        bool full = true;

        foreach (Weapon w in meleeWeapons)
        {
            if (w == null)
            {
                full = false;
                break;
            }                
        }

        return full;
    }
}
