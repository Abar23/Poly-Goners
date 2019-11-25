using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class Player : MonoBehaviour
{
    public int PlayerNumber;
    public Player OtherPlayer;
    public float MoveSpeed = 1.0f;
    public float RotateSpeed = 1.0f;
    public float Gravity = 14.0f;
    public float JumpSpeed = 10.0f;
    public float CrosshairDistance = 3.0f;
    public GameObject Crosshair;
    public GameObject RevivePrompt;
    public Image RevivePromptFill;
    public Image MeleeDropFill;
    public Image MagicDropFill;
    public Image PotionDropFill;

    private Stamina stamina;
    private MagicBox magicBox;
    private Inventory inventory;
    private CharacterController character;
    private Animator animator;
    private AnimatorOverrideController animatorOverrideController;
    private bool lockAim = false;
    private bool setEnemy = false;
    private Vector3 lookDir;
    private bool isPaused;

    private float reviveTimer;
    private float timeToRevive = 3f;
    private float reviveDistance = 2.5f;
    private float rollStaminaCost = 20f;
    private float jumpStaminaCost = 15f;

    private float meleeDropTimer;
    private float magicDropTimer;
    private float potionDropTimer;
    private float timeToDrop = 2f;
    private bool meleeDropped = false;
    private bool magicDropped = false;

    private bool isAlive = true;

    public GameObject DeathTimer;
    public Image DeathTimerFill;
    private bool permaDead = false;
    private bool isReviving = false;
    private float timeToDie = 60f;
    private float deathTimeRemaining;

    private Vector3 lastGroundedPosition;

    public Vector3 MoveDir { get; private set; }
    public float VerticalVelocity { get; private set; }
    public IController Controller { get; private set; }
    public PlayerMovementState PlayerMovementState { get; private set; }
    public Weapon currentWeapon { get; private set; }

    private WeaponManager weaponManager;

    private int previousLayer = -1;

    public UnityEvent OnMeleeAttack;

    [SerializeField] private AudioSource m_ReviveSFX;

    private void Awake()
    {
        magicBox = GetComponentInChildren<MagicBox>();
        weaponManager = GetComponentInChildren<WeaponManager>();
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        PlayerMovementState = new PlayerGroundedState(this, animator);
        RevivePrompt.SetActive(false);
        DeathTimer.SetActive(false);
        character = GetComponent<CharacterController>();
        inventory = GetComponent<Inventory>();
        stamina = GetComponent<Stamina>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;
        lookDir = transform.forward;
        reviveTimer = 0f;
        meleeDropTimer = 0f;
        magicDropTimer = 0f;
        potionDropTimer = 0f;
        deathTimeRemaining = timeToDie;
    }

    private void FixedUpdate()
    {
        if (PlayerPrefs.HasKey("fx"))
            m_ReviveSFX.volume = PlayerPrefs.GetFloat("fx");

        if (transform.position.y < -7.5f)
        {
            GetComponent<Damageable>().TakeFallDamage();
            transform.position = lastGroundedPosition;
            VerticalVelocity = 0f;
        }
    }

    private void Update()
    {
        if (PlayerNumber == 1)
            Controller = ControllerManager.GetInstance().GetComponent<ControllerManager>().GetPlayerOneController();
        else
            Controller = ControllerManager.GetInstance().GetComponent<ControllerManager>().GetPlayerTwoController();

        if (!(Controller is NullController))
        {
            if (!(PlayerMovementState is PlayerDeathState) && !isPaused) // this player is not dead
            {
                UpdateInput();
                PlayerMovementState.Update();

                if (currentWeapon != null)
                {
                    int currentLayer = weaponManager.GetWeaponConfig().GetAnimationLayer();
                    if (currentLayer != previousLayer)
                    {
                        if (previousLayer != -1)
                            animator.SetLayerWeight(previousLayer, 0);
                        previousLayer = currentLayer;
                    }
                    else
                    {
                        animator.SetLayerWeight(currentLayer, 1);
                    }
                }
                else if (previousLayer != -1)
                {
                    animator.SetLayerWeight(previousLayer, 0);
                }
            }
            else // this player is dead
            {
                if (!permaDead && !isReviving)
                {
                    DeathTimerFill.fillAmount = deathTimeRemaining / timeToDie;
                    deathTimeRemaining -= Time.deltaTime;

                    if (deathTimeRemaining <= 0)
                    {
                        permaDead = true;
                        inventory.DropCoin();
                        StartCoroutine(DisablePlayer());
                    }

                }

                if (OtherPlayer.PlayerMovementState is PlayerDeathState) // both players are dead
                {
                    RevivePrompt.gameObject.SetActive(false);
                    DeathTimer.gameObject.SetActive(false);
                }
            }
        }
    }

    private void UpdateInput()
    {
        if (!animator.GetBool("isSpinning") && !(animator.GetCurrentAnimatorStateInfo(0).IsName("End Spin")))
        {
            HandleMove();
            HandleRotation();
        }

        if (Controller.GetControllerActions().dPadDown.WasPressed)
        {
            GetComponent<CharacterBox>().NextCharacter();
            UpdateAnimator();
        }



        float dist = Vector3.Distance(transform.position, OtherPlayer.transform.position);

        // Check if able to revive the other player
        if (dist < reviveDistance && OtherPlayer.PlayerMovementState is PlayerDeathState && !OtherPlayer.IsPermaDead())
        {
            OtherPlayer.RevivePrompt.gameObject.SetActive(true);
            OtherPlayer.RevivePromptFill.fillAmount = reviveTimer / timeToRevive;

            if (Controller.GetControllerActions().action3.IsPressed)
            {
                isReviving = true;
                reviveTimer += Time.deltaTime;

                if (reviveTimer > timeToRevive)
                {
                    OtherPlayer.PlayerMovementState.HandleGroundedTransition();
                    OtherPlayer.GetComponent<Damageable>().RevivePlayer();
                    OtherPlayer.OnRevive();
                }
            }
            else
            {
                isReviving = false;
                if (reviveTimer > 0f)
                    reviveTimer -= Time.deltaTime * 2f;
            }
        }
        else if (dist >= reviveDistance && OtherPlayer.PlayerMovementState is PlayerDeathState && !OtherPlayer.IsPermaDead())
        {
            OtherPlayer.RevivePrompt.gameObject.SetActive(false);
            reviveTimer = 0f;
        }

        // Perform melee attack
        if (Controller.GetControllerActions().rightBumper.WasPressed && currentWeapon != null && !IsMagicCasting())
        {
            float attackStamina = weaponManager.GetWeaponConfig().GetPrimaryAttackStamina();
            if (!currentWeapon.CheckIfAttacking() && stamina.CurrentStaminaValue() > attackStamina)
            {
                stamina.DecreaseStamina(attackStamina);
                animatorOverrideController["PRIMARY_ATTACK"] = weaponManager.GetWeaponConfig().GetPrimaryAttackAnimation();
                animator.SetTrigger("PrimaryAttackTrigger");
                currentWeapon.SwingWeapon(animator.GetCurrentAnimatorStateInfo(6).length);
                TriggerEvent(OnMeleeAttack);
            }
        }

        //  MELEE SPIN MOVE
        if (Controller.GetControllerActions().rightTrigger.IsPressed && currentWeapon != null && stamina.CurrentStaminaValue() > 0 && character.isGrounded)
        {
            currentWeapon.SwingWeapon(1f);
            currentWeapon.SpinCollider();
            animator.SetBool("isSpinning", true);
            if(!(animator.GetCurrentAnimatorStateInfo(0).IsName("Start Spin") || animator.GetAnimatorTransitionInfo(0).IsName("Moving -> Start Spin")))
                stamina.DecreaseStamina(1.2f);
        }
        else
        {
            animator.SetBool("isSpinning", false);
            if (currentWeapon != null && !currentWeapon.CheckIfAttacking())
            {
                currentWeapon.DefaultCollider();
                currentWeapon.ChangeParticles(false);
            }                
        }

        // HOLD MAGIC ATTACK
        if (Controller.GetControllerActions().leftTrigger.IsPressed && !IsSpinning() && !IsRolling())
        {
            animator.SetBool("holdCast", true);
        }
        else
        {
            animator.SetBool("holdCast", false);
        }

        // Perform magic attack
        if (Controller.GetControllerActions().leftBumper.WasPressed)
        {
            if (inventory.UseMagic())
            {
                animator.SetTrigger("CastTrigger");
            }
        }

        // Stop magic attack
        if (Controller.GetControllerActions().leftBumper.WasReleased)
        {
            if (inventory.StopMagic())
            {
                // Deactivate magic anim
            }
        }

        // Use Potion
        if (Controller.GetControllerActions().action4.WasPressed && inventory.HasPotion())
        {
            animator.SetTrigger("DrinkPotion");
            inventory.UsePotion();
        }

        // Drop melee weapon
        MeleeDropFill.fillAmount = meleeDropTimer / timeToDrop;
        if (Controller.GetControllerActions().dPadRight.WasReleased && !meleeDropped)
        {
            animator.SetTrigger("SwitchWeapons");
        }
        else if (currentWeapon != null && Controller.GetControllerActions().dPadRight.IsPressed)
        {
            meleeDropTimer += Time.deltaTime;

            if (meleeDropTimer > timeToDrop)
            {
                inventory.DropWeapon();
                animator.SetTrigger("DropItem");
                meleeDropTimer = 0f;
                meleeDropped = true;
            }
        }
        else if (Controller.GetControllerActions().dPadRight.WasReleased && meleeDropped)
        {
            meleeDropped = false;
        }
        else
        {
            if (meleeDropTimer > 0f)
                meleeDropTimer -= Time.deltaTime * 2f;
        }

        // Drop magic weapon
        MagicDropFill.fillAmount = magicDropTimer / timeToDrop;
        if (Controller.GetControllerActions().dPadLeft.WasReleased && !magicDropped)
        {
            inventory.NextMagicWeapon();
        }
        else if (inventory.MagicEquipped() && Controller.GetControllerActions().dPadLeft.IsPressed)
        {
            magicDropTimer += Time.deltaTime;

            if (magicDropTimer > timeToDrop)
            {
                inventory.DropMagic();
                animator.SetTrigger("DropItem");
                magicDropTimer = 0f;
                magicDropped = true;
            }
        }
        else if (Controller.GetControllerActions().dPadLeft.WasReleased && magicDropped)
        {
            magicDropped = false;
        }
        else
        {
            if (magicDropTimer > 0f)
                magicDropTimer -= Time.deltaTime * 2f;
        }

        // Drop potion
        PotionDropFill.fillAmount = potionDropTimer / timeToDrop;
        if (inventory.HasPotion() && Controller.GetControllerActions().dPadUp.IsPressed)
        {
            potionDropTimer += Time.deltaTime;

            if (potionDropTimer > timeToDrop)
            {
                inventory.DropPotion();
                animator.SetTrigger("DropItem");
                potionDropTimer = 0f;
            }
        }
        else
        {
            if (potionDropTimer > 0f)
                potionDropTimer -= Time.deltaTime * 2f;
        }

        // Lock on 
        if (Controller.GetControllerActions().rightStickClick.WasPressed)
        {
            lockAim = !lockAim;
        }

        // Cheats
        if (Controller.GetControllerActions().cheatMagicRefill.WasPressed)
        {
            magicBox.ResetMagicToFull();
        }
    }

    public void ChangeCurrentWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        meleeDropped = false;
    }

    public void ChangeMovementState(PlayerMovementState state)
    {
        PlayerMovementState = state;
    }

    float shortestDist = 100f;
    Collider closestEnemy = new Collider();
    float lastDist = 0f;
    public void HandleRotation()
    {
        if (!lockAim)
        {
            Crosshair.SetActive(false);
            lastDist = 0f;
            shortestDist = 100f;
            setEnemy = false;
            if (closestEnemy != new Collider() && closestEnemy != null) // turn off target above enemy
            {
                Component[] icons = closestEnemy.gameObject.GetComponentsInChildren<DisableOnStart>();
                foreach (Component c in icons)
                {
                    if ((PlayerNumber == 1 && c.gameObject.name == "Player1Target") || (PlayerNumber == 2 && c.gameObject.name == "Player2Target"))
                        c.gameObject.GetComponent<Image>().enabled = false;
                }
                closestEnemy = null;
            }

            if (Controller.isUsingKeyboard())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    lookDir = hit.point - transform.position;
                }
            }
            else
            {
                lookDir = Vector3.right * Controller.GetControllerActions().look.X + Vector3.forward * Controller.GetControllerActions().look.Y;
            }
        }
        else if (!setEnemy) // find closest enemy in field of view
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 7.5f);
            foreach (Collider hit in hitColliders)
            {
                if (hit.gameObject.tag == "Enemy")
                {
                    float dist = Vector3.Distance(transform.position, hit.gameObject.transform.position);
                    float angleToEnemy = Vector3.Angle(transform.forward, hit.gameObject.transform.position - transform.position);
                    // check enemy is in front of player, 160 degree FOV
                    if (angleToEnemy >= -80f && angleToEnemy <= 80f && dist < shortestDist)
                    {
                        shortestDist = dist;
                        closestEnemy = hit;
                    }
                }
            }

            if (shortestDist != 100f) // lock on to closest enemy
            {
                setEnemy = true;
                lookDir = (closestEnemy.gameObject.transform.position - transform.position).normalized;
            }
            else
            {
                lockAim = false;
            }
        }
        else // continue locking to same enemy
        {
            if (closestEnemy != null && lastDist < 10f) // enemy is not dead and within the range of the player
            {
                lastDist = Vector3.Distance(transform.position, closestEnemy.gameObject.transform.position);
                lookDir = (closestEnemy.gameObject.transform.position - transform.position).normalized;
                Component[] icons = closestEnemy.gameObject.GetComponentsInChildren<DisableOnStart>();
                foreach (Component c in icons)
                {
                    if ((PlayerNumber == 1 && c.gameObject.name == "Player1Target") || (PlayerNumber == 2 && c.gameObject.name == "Player2Target"))
                        c.gameObject.GetComponent<Image>().enabled = true;
                }
            }
            else
            {
                lockAim = false;
            }
        }

        Vector3 moveDir = Vector3.right * Controller.GetControllerActions().move.X + Vector3.forward * Controller.GetControllerActions().move.Y;
        if (lookDir.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir, Vector3.up), RotateSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            if (!lockAim)
            {
                Crosshair.SetActive(true);
                Crosshair.transform.localPosition = new Vector3(0f, 0f, CrosshairDistance);
            }
            else
            {
                Crosshair.SetActive(false);
            }
        }
        else if (moveDir.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir, Vector3.up), RotateSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            Crosshair.SetActive(false);
        }
    }


    public void HandleMove()
    {
        MoveDir = new Vector3(Controller.GetControllerActions().move.X, 0f, Controller.GetControllerActions().move.Y);

        if (character.isGrounded)
        {
            MoveDir *= MoveSpeed;
            PlayerMovementState.HandleGroundedTransition();

            // Handle Roll Input
            if (Controller.GetControllerActions().action2.WasPressed && !IsRolling() && stamina.CurrentStaminaValue() >= rollStaminaCost)
            {
                stamina.DecreaseStamina(rollStaminaCost);
                PlayerMovementState.HandleRollingTransition();
            }

            // Handle Jump Input
            VerticalVelocity = -Gravity * Time.deltaTime;
            if (Controller.GetControllerActions().action1.WasPressed && !(animator.GetCurrentAnimatorStateInfo(0).IsName("Land") || animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || IsRolling()) && stamina.CurrentStaminaValue() >= jumpStaminaCost)
            {
                stamina.DecreaseStamina(jumpStaminaCost);
                PlayerMovementState.HandleJumpingTransition();
                VerticalVelocity = JumpSpeed;
            }

            lastGroundedPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            lastGroundedPosition -= (transform.forward / 2);
        }
        else
        {
            MoveDir *= MoveSpeed;
            VerticalVelocity -= Gravity * Time.deltaTime;
        }

        MoveDir = new Vector3(MoveDir.x, VerticalVelocity, MoveDir.z);
        character.Move(MoveDir * Time.deltaTime);
    }

    private bool IsRolling()
    {
        return animator.GetAnimatorTransitionInfo(0).IsName("Moving -> Roll Forward") || animator.GetAnimatorTransitionInfo(0).IsName("Moving -> Roll Back")
            || animator.GetAnimatorTransitionInfo(0).IsName("Moving -> Roll Right") || animator.GetAnimatorTransitionInfo(0).IsName("Moving -> Roll Left")
            //|| animator.GetAnimatorTransitionInfo(0).IsName("Roll Forward -> Moving") || animator.GetAnimatorTransitionInfo(0).IsName("Roll Back -> Moving")
            //|| animator.GetAnimatorTransitionInfo(0).IsName("Roll Right -> Moving") || animator.GetAnimatorTransitionInfo(0).IsName("Roll Left -> Moving")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Roll Forward") || animator.GetCurrentAnimatorStateInfo(0).IsName("Roll Back")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Roll Right") || animator.GetCurrentAnimatorStateInfo(0).IsName("Roll Left");
    }

    private bool IsSpinning()
    {
        return animator.GetAnimatorTransitionInfo(0).IsName("Moving -> Start Spin") || animator.GetAnimatorTransitionInfo(0).IsName("Start Spin -> Spin")
            || animator.GetAnimatorTransitionInfo(0).IsName("Spin -> End Spin") || animator.GetAnimatorTransitionInfo(0).IsName("End Spin -> Moving")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Start Spin") || animator.GetCurrentAnimatorStateInfo(0).IsName("End Spin")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Spin");
    }

    private bool IsMagicCasting()
    {
        return animator.GetAnimatorTransitionInfo(6).IsName("New State -> Magic Cast Start") || animator.GetAnimatorTransitionInfo(6).IsName("Magic Cast Start -> Magic Cast Loop")
            || animator.GetAnimatorTransitionInfo(6).IsName("Magic Cast Loop -> Magic Cast End") || animator.GetAnimatorTransitionInfo(6).IsName("Magic Cast End -> New State")
            || animator.GetCurrentAnimatorStateInfo(6).IsName("Magic Cast Start") || animator.GetCurrentAnimatorStateInfo(6).IsName("Magic Cast End")
            || animator.GetCurrentAnimatorStateInfo(6).IsName("Magic Cast Loop");
    }

    void TriggerEvent(UnityEvent uEvent)
    {
        if (uEvent != null)
        {
            uEvent.Invoke();
        }
    }

    public bool CheckUseButtonPress()
    {
        if (Controller.GetControllerActions().action3.WasPressed)
            return true;
        else
            return false;
    }

    public void OnDeath()
    {
        character.enabled = false;
        PlayerMovementState.HandleDeathTransition();
        isAlive = false;
        Crosshair.SetActive(false);
        if (OtherPlayer.isActiveAndEnabled)
        {
            DeathTimer.SetActive(true);
        }
        inventory.StopMagic();
    }

    public void OnRevive()
    {
        character.enabled = true;
        isAlive = true;
        if (permaDead && !gameObject.activeSelf)
        {
            permaDead = false;
            gameObject.SetActive(true);
        }

        deathTimeRemaining = timeToDie;
        reviveTimer = 0;
        DeathTimer.SetActive(false);
        m_ReviveSFX.Play();
    }

    public bool IsDead()
    {
        return !isAlive;
    }

    public bool IsPermaDead()
    {
        return permaDead;
    }

    public void SetIsPaused(bool paused)
    {
        isPaused = paused;
    }

    IEnumerator DisablePlayer()
    {
        yield return new WaitForSeconds(5);
        this.gameObject.SetActive(false);
    }
    
    public void UpdateAnimator()
    {
        animator = GetComponentInChildren<Animator>();
        PlayerMovementState.UpdateAnimator(animator);
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;
    }
}
