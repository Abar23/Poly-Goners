using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int NumberOfMeleeSlots;
    public int NumberOfMagicSlots;
    public Text CoinDisplay;
    public IconManager MeleeIcon;
    public IconManager MagicIcon;
    public IconManager PotionIcon;
    public List<GameObject> WeaponPrefabs;

    private Player player;
    private WeaponManager weaponManager;
    private Damageable damageable;
    private MagicBox magicBox;
    private bool isStrengthed = false;

    private Weapon[] meleeWeapons;
    private Projectile[] magicAbilities;
    private Collectable potion;
    private int gold;

    private int currentMeleeIndex;
    private int currentMagicIndex;

    void Awake()
    {
        damageable = GetComponent<Damageable>();
        magicBox = GetComponentInChildren<MagicBox>();
    }

    void Start()
    {
        player = GetComponent<Player>();
        weaponManager = GetComponentInChildren<WeaponManager>();
        potion = null;

        meleeWeapons = new Weapon[NumberOfMeleeSlots];
        magicAbilities = new Projectile[NumberOfMagicSlots];
        currentMeleeIndex = 0;
        currentMagicIndex = 0;
        MagicIcon.EnableIcon(currentMagicIndex.ToString());
    }

    public int GetGold()
    {
        return gold;
    }

    public void IncreaseGold(int num)
    {
        gold += num;
        CoinDisplay.text = gold.ToString();
    }

    public void DecreaseGold(int num)
    {
        gold -= num;
        CoinDisplay.text = gold.ToString();
    }

    public void UsePotion()
    {
        if (HasPotion())
        {
            PotionConfig config = potion.GetComponent<Collectable>().Config;
            if (config is HealthPotionConfig)
            {
                StartCoroutine(HealthPotionEffect(damageable, (HealthPotionConfig)config));
            }
            else if (config is MagicPotionConfig)
            {
                StartCoroutine(MagicPotionEffect(magicBox, (MagicPotionConfig)config));
            }
            else if (config is StrengthPotionConfig)
            {
                if (isStrengthed) return;
                Damager[] damagers = GetComponentsInChildren<Damager>();
                foreach (Damager damager in damagers)
                {
                    damager.SetMultiplier(((StrengthPotionConfig)config).Multiplier);
                }
                Invoke("ResetMultiplier", config.EffectiveTime);
            }

            Destroy(potion);
            PotionIcon.DisableCurrentIcon();
        }
    }

    public bool UseMagic()
    {
        bool canFire = magicBox.FireMagic(currentMagicIndex);
        return canFire;
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
        int totalNumberOfSpells = magicBox.GetNumberOfSpells();
        currentMagicIndex = (currentMagicIndex - 1 + totalNumberOfSpells) % totalNumberOfSpells;
        MagicIcon.EnableIcon(currentMagicIndex.ToString());
    }

    public void AddMeleeWeapon(Weapon weapon)
    {
        bool added = false;
        for (int i = 0; i < NumberOfMeleeSlots && !added; i++)
        {
            if (meleeWeapons[i] == null)
            {
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

    public void DropMagic()
    {

    }

    public void DropPotion()
    {
        GameObject p = Instantiate(potion.gameObject, transform.position + transform.forward * 2f, Quaternion.identity);
        p.transform.localScale = new Vector3(2f, 2f, 2f);
        p.SetActive(true);
        PotionIcon.DisableCurrentIcon();
        Destroy(potion);
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

    public bool HasPotion()
    {
        return (potion != null);
    }

    void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;
        Collectable collectable = obj.GetComponent<Collectable>();
        if (collectable == null) return;
        if (collectable.CollectableType == Collectable.Type.Coin)
        {
            IncreaseGold(1);
        }
        else if (collectable.CollectableType == Collectable.Type.Potion)
        {
            if (!HasPotion())
            {
                AddPotionToInventory(collectable);
            }
        }
    }

    public void AddPotionToInventory(Collectable p)
    {
        potion = p;
        potion.gameObject.SetActive(false);
        PotionConfig config = potion.GetComponent<Collectable>().Config;
        if (config is HealthPotionConfig)
        {
            PotionIcon.EnableIcon("BigRed");
        }
        else if (config is MagicPotionConfig)
        {
            PotionIcon.EnableIcon("BigBlue");
        }
        else if (config is StrengthPotionConfig)
        {
            PotionIcon.EnableIcon("BigPurple");
        }
    }

    IEnumerator HealthPotionEffect(Damageable damageable, HealthPotionConfig config)
    {
        float elapse = 0f;
        while (elapse < config.EffectiveTime)
        {
            damageable.IncreaseHealth(config.Amount);
            elapse += config.Interval;
            yield return new WaitForSeconds(config.Interval);
        }
    }

    IEnumerator MagicPotionEffect(MagicBox magicBox, MagicPotionConfig config)
    {
        float elapse = 0f;
        while (elapse < config.EffectiveTime)
        {
            magicBox.IncreaseMagicPoint(config.Amount);
            elapse += config.Interval;
            yield return new WaitForSeconds(config.Interval);
        }
    }

    void ResetMultiplier()
    {
        Damager[] damagers = GetComponentsInChildren<Damager>();
        foreach (Damager damager in damagers)
        {
            damager.SetMultiplier(1f);
        }
    }
}
