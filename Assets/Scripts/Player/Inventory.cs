using System.Collections;
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

    public Image StrengthIndicator;
    public Image StrengthTimerCircle;
    private bool strengthPotionUsed = false;
    private float potionTime;
    private float remainingTime;

    [SerializeField] private AudioSource m_PotionSFX;
    [SerializeField] private AudioSource m_PurchaseSFX;

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

    private void Update()
    {
        if (strengthPotionUsed)
        {
            StrengthTimerCircle.fillAmount = remainingTime / potionTime;
            remainingTime -= Time.deltaTime;
        }
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
        m_PurchaseSFX.Play();
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
                potionTime = config.EffectiveTime;
                remainingTime = config.EffectiveTime;
                strengthPotionUsed = true;
                Invoke("ResetMultiplier", config.EffectiveTime);
            }
            m_PotionSFX.Play();
            Destroy(potion.gameObject);
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
                DontDestroyOnLoad(pickup);
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
        meleeDropables[currentMeleeIndex].SetActive(true);

        GameObject newMelee = Instantiate(meleeDropables[currentMeleeIndex], player.transform.position + player.transform.forward, Quaternion.identity);
        newMelee.name = meleeDropables[currentMeleeIndex].name;
        Destroy(meleeDropables[currentMeleeIndex]);

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
                DontDestroyOnLoad(pickup);
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
        Vector3 newPos = new Vector3(player.transform.position.x + player.transform.forward.x, player.transform.position.y + .5f, player.transform.position.z + player.transform.forward.z);
        magicDropables[currentMagicIndex].SetActive(true);
        GameObject newMagic = Instantiate(magicDropables[currentMagicIndex], newPos, Quaternion.Euler(0, 0, 0));
        newMagic.name = magicDropables[currentMagicIndex].name;
        Destroy(magicDropables[currentMagicIndex]);

        magicAbilities[currentMagicIndex] = null;
        magicDropables[currentMagicIndex] = null;
    }

    public void DropPotion()
    {
        potion.transform.localScale = new Vector3(2f, 2f, 2f);
        potion.gameObject.SetActive(true);
        GameObject newPotion = Instantiate(potion.gameObject, transform.position + transform.forward * 2f, Quaternion.identity);
        newPotion.name = potion.name;
        Destroy(potion.gameObject);
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

    public void OnDeath()
    {
        gold /= 2;
        CoinDisplay.text = gold.ToString();

        for (int i = 0; i < NumberOfMagicSlots; i++)
        {
            magicAbilities[i] = null;
            magicDropables[i] = null;
        }

        for (int i = 0; i < NumberOfMeleeSlots; i++)
        {
            meleeWeapons[i] = null;
            meleeDropables[i] = null;
        }

        potion = null;

        MeleeIcon.DisableCurrentIcon();
        MagicIcon.DisableCurrentIcon();
        PotionIcon.DisableCurrentIcon();
        weaponManager.UnequipCurrentWeapon();
        player.ChangeCurrentWeapon(meleeWeapons[currentMeleeIndex]);
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
            if (!HasPotion() && player.CheckUseButtonPress())
            {
                GameObject newPotion = Instantiate(obj);
                newPotion.name = newPotion.name.Substring(0, newPotion.name.Length - 7); // remove (clone) from name
                AddPotionToInventory(newPotion.GetComponent<Collectable>());
                Destroy(obj);
            }
            else if (HasPotion() && player.CheckUseButtonPress()) 
            {
                collectable.gameObject.GetComponent<PickupItemLabel>().ShowInventoryFullText();
            }
        }
    }

    public void AddPotionToInventory(Collectable p)
    {
        potion = p;
        DontDestroyOnLoad(potion.gameObject);
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
        strengthPotionUsed = false;
        foreach (Damager damager in damagers)
        {
            damager.SetMultiplier(1f);
        }
    }

    public static void DontDestroyChildOnLoad(GameObject child)
    {
        Transform parentTransform = child.transform;

        // If this object doesn't have a parent then its the root transform.
        while (parentTransform.parent != null)
        {
            // Keep going up the chain.
            parentTransform = parentTransform.parent;
        }
        GameObject.DontDestroyOnLoad(parentTransform.gameObject);
    }
}
