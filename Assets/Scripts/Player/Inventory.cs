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

        if (meleeDropables[currentMeleeIndex].tag == "ShopWeapon")
        {
            meleeDropables[currentMeleeIndex].tag = "Untagged";
            meleeDropables[currentMeleeIndex].AddComponent<WeaponPickup>();
            meleeDropables[currentMeleeIndex].AddComponent<ItemPickupEffect>();
            meleeDropables[currentMeleeIndex].AddComponent<BoxCollider>();
            meleeDropables[currentMeleeIndex].GetComponent<BoxCollider>().isTrigger = true;
            meleeDropables[currentMeleeIndex].GetComponent<ShopItem>().enabled = false;
            meleeDropables[currentMeleeIndex].GetComponentInChildren<Canvas>().enabled = false;
            meleeDropables[currentMeleeIndex].name = meleeDropables[currentMeleeIndex].name + " Pickup";
        }    

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
        //Vector3 newPos = new Vector3(player.transform.position.x + player.transform.forward.x, -2f, player.transform.position.z + player.transform.forward.z);
        Vector3 newPos = new Vector3(player.transform.position.x + player.transform.forward.x, player.transform.position.y + .75f, player.transform.position.z + player.transform.forward.z);
        magicDropables[currentMagicIndex].transform.position = newPos;
        magicDropables[currentMagicIndex].SetActive(true);

        magicAbilities[currentMagicIndex] = null;
        magicDropables[currentMagicIndex] = null;
    }

    public void DropPotion()
    {
        potion.transform.position = transform.position + transform.forward * 2f;
        potion.transform.localScale = new Vector3(2f, 2f, 2f);
        potion.gameObject.SetActive(true);
        PotionIcon.DisableCurrentIcon();
        potion = null;
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
                AddPotionToInventory(collectable);
                potion = collectable;
                potion.gameObject.SetActive(false);
                PotionConfig config = potion.GetComponent<Collectable>().Config;
                PotionIcon.EnableIcon(potion.gameObject);
            }
        }
    }

    public void AddPotionToInventory(Collectable p)
    {
        potion = p;
        if (potion.tag == "ShopPotion")
        {
            potion.tag = "Untagged";
            potion.GetComponent<ShopItem>().enabled = false;
            potion.GetComponentInChildren<Canvas>().enabled = false;
            potion.gameObject.AddComponent<MeshCollider>();
            potion.gameObject.GetComponent<MeshCollider>().convex = true;
            potion.gameObject.GetComponent<MeshCollider>().isTrigger = true;
        }
        potion.gameObject.SetActive(false);

        PotionIcon.EnableIcon(p.gameObject);
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
