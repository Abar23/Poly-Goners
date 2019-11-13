using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [HideInInspector]
    public bool swingingWeapon = false;
    private bool ableToHitEnemy = false;
    private float swingTime;
    private float elapsedTime;
    private Collider collider;
    public WeaponConfig weaponConfig;

    public void Start()
    {
        this.transform.localPosition = weaponConfig.GetStartPosition();
        this.transform.localEulerAngles = weaponConfig.GetStartRotation();

        GetComponentInParent<Player>().ChangeCurrentWeapon(this);
        collider = GetComponent<Collider>();
    }

    public void Update()
    {
        if (swingingWeapon)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > swingTime)
            {
                elapsedTime = 0;
                swingingWeapon = false;
                this.GetComponent<Damager>().enabled = true;
            }
        }
    }

    public void LateUpdate()
    {
        if (!swingingWeapon && collider.enabled)
        {
            collider.enabled = false;
        }
    }

    public void SwingWeapon(float animationTime)
    {
        swingingWeapon = true;
        ableToHitEnemy = true;
        swingTime = animationTime;
    }

    public bool CheckIfAttacking()
    {
        return swingingWeapon;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (ableToHitEnemy)
        {
            ableToHitEnemy = false;
            this.GetComponentInChildren<Damager>().enabled = false;
        }
    }

    public WeaponConfig GetConfig()
    {
        return weaponConfig;
    }
}
