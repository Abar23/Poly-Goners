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

    private ParticleSystem[] pss;
    private BoxCollider box;
    private Vector3 defaultSize;
    private Vector3 fullSize;

    public void Start()
    {
        this.transform.localPosition = weaponConfig.GetStartPosition();
        this.transform.localEulerAngles = weaponConfig.GetStartRotation();

        Player player = GetComponentInParent<Player>();
        if (player != null)
            player.ChangeCurrentWeapon(this);
        collider = GetComponent<Collider>();

        pss = GetComponentsInChildren<ParticleSystem>();
        ChangeParticles(false);

        box = GetComponent<BoxCollider>();
        defaultSize = box.size;
        fullSize = new Vector3(defaultSize.x, defaultSize.y * 1.25f, defaultSize.z);
        box.size = fullSize;
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
            ChangeParticles(false);
        }
    }

    public void SwingWeapon(float animationTime)
    {
        swingingWeapon = true;
        ableToHitEnemy = true;
        swingTime = animationTime - (animationTime * 0.15f);

        ChangeParticles(true);
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

    public void ChangeParticles(bool enabled)
    {
        if (pss != null)
        {
            foreach (ParticleSystem ps in pss)
            {
                var em = ps.emission;
                em.enabled = enabled;
            }
        }
    }

    public void SpinCollider()
    {
        if (box != null)
            box.size = fullSize;
    }

    public void DefaultCollider()
    {
        if (box != null)
            box.size = fullSize;
    }
}
