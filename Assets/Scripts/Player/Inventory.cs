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
    public Image StrengthIndicator;

    private Player player;
    private WeaponManager weaponManager;
    private Damageable damageable;
    private MagicBox magicBox;
    private bool isStrengthed = false;

    private Weapon[] meleeWeapons;
    private GameObject[] meleeDropables;
    private string[] magicAbilities;
    private GameObject[] magicDropables;
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
        StrengthIndicator.gameObject.SetActive(false);
        player = GetComponent<Player>();
        weaponManager = GetComponentInChildren<WeaponManager>();
        potion = null;

        meleeWeapons = new Weapon[NumberOfMeleeSlots];
        magicAbilities = new string[NumberOfMagicSlots];
        meleeDropables = new GameObject[NumberOfMeleeSlots];
        magicDropables = new GameObject[NumberOfMagicSlots];
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
                StrengthIndicator.gameObject.SetActive(true);
                Invoke("ResetMultiplier", config.EffectiveTime);
            }

            Destroy(potion);
            PotionIcon.DisableCurrentIcon();
        }
    }

    public bool MagicEquipped()
    {
        if (magicAbilities[currentMagicIndex] == null)
            return false;
        else
            return true;
    }

    public bool UseMagic()
    {
        if (magicAbilities[currentMagicIndex] != null)
        {
            bool canFire = magicBox.FireMagic(magicBox.GetIndexFromName(magicAbilities[currentMagicIndex]));
            return canFire;
        }
        else
            return false;
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
            MeleeIcon.EnableIcon(meleeDropables[currentMeleeIndex].gameObject);
        }

        player.ChangeCurrentWeapon(meleeWeapons[currentMeleeIndex]);
    }

    public void NextMagicWeapon()
    {
        currentMagicIndex++;
        if (currentMagicIndex == NumberOfMagicSlots)
            currentMagicIndex = 0;

        MagicIcon.DisableCurrentIcon();

        if (magicAbilities[currentMagicIndex] != null)
        {
            MagicIcon.EnableIcon(magicDropables[currentMagicIndex]);
        }
    }

    public void AddMeleeWeapon(Weapon weapon, GameObject pickup)
    {
        bool added = false;
        for (int i = 0; i < NumberOfMeleeSlots && !added; i++)
        {
            if (meleeWeapons[i] == null)
            {
                meleeWeapons[i] = weapon;
                meleeDropables[i] = pickup;
                currentMeleeIndex = i;
                MeleeIcon.EnableIcon(meleeDropables[currentMeleeIndex]);
                player.ChangeCurrentWeapon(meleeWeapons[currentMeleeIndex]);
                added = true;
            }
        }
    }

    public void DropWeapon()
    {
        weaponManager.UnequipCurrentWeapon();
        MeleeIcon.DisableCurrentIcon();
        meleeDropables[currentMeleeIndex].transform.position = player.transform.position + player.transform.forward;
        meleeDropables[currentMeleeIndex].SetActive(true);

        meleeWeapons[currentMeleeIndex] = null;
        meleeDropables[currentMeleeIndex] = null;
        player.ChangeCurrentWeapon(meleeWeapons[currentMeleeIndex]);
    }

    public void AddMagicAbility(string magicName, GameObject pickup)
    {
        bool added = false;
        for (int i = 0; i < NumberOfMagicSlots && !added; i++)
        {
            if (magicAbilities[i] == null)
            {
                magicAbilities[i] = magicName;
                magicDropables[i] = pickup;
                currentMagicIndex = i;
                MagicIcon.EnableIcon(magicDropables[i]);
                added = true;
            }
        }
    }

    public void DropMagic()
    {
        MagicIcon.DisableCurrentIcon();
        Vector3 newPos = new Vector3(player.transform.position.x + player.transform.forward.x, -2f, player.transform.position.z + player.transform.forward.z);
        magicDropables[currentMagicIndex].transform.position = newPos;
        magicDropables[currentMagicIndex].SetActive(true);

        magicAbilities[currentMagicIndex] = null;
        magicDropables[currentMagicIndex] = null;
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

    public bool IsMagicFull()
    {
        bool full = true;

        foreach (string p in magicAbilities)
        {
            if (p == null)
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
                potion.gameObject.SetActive(false);
                PotionConfig config = potion.GetComponent<Collectable>().Config;
                PotionIcon.EnableIcon(potion.gameObject);
            }
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
        StrengthIndicator.gameObject.SetActive(false);
        foreach (Damager damager in damagers)
        {
            damager.SetMultiplier(1f);
        }
    }
}
