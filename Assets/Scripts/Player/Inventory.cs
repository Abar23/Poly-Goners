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
        }
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
                potion = collectable;
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
        }
    }

    IEnumerator HealthPotionEffect(Damageable damageable, HealthPotionConfig config)
    {
        float elapse = 0f;
        while (elapse < config.EffectiveTime)
        {
            damageable.IncreaseHealth(config.Amount);
            yield return new WaitForSeconds(config.Interval);
            elapse += Time.deltaTime;
        }
    }

    IEnumerator MagicPotionEffect(MagicBox magicBox, MagicPotionConfig config)
    {
        float elapse = 0f;
        while (elapse < config.EffectiveTime)
        {
            magicBox.IncreaseMagicPoint(config.Amount);
            yield return new WaitForSeconds(config.Interval);
            elapse += Time.deltaTime;
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
