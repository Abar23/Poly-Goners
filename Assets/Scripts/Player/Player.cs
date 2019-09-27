using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int PlayerNumber;
    public float MoveSpeed = 1.0f;
    public float RotateSpeed = 1.0f;
    public float Gravity = 14.0f;
    public float JumpSpeed = 10.0f;
    public float CrosshairDistance = 3.0f;
    public GameObject Crosshair;

    #region MagicCast
    private MagicBox magicBox;
    #endregion

    private PlayerMovementState playerMovementState;
    private CharacterController character;
    private Animator animator;
    private float verticalVelocity;
    private bool lockAim = false;
    private Vector3 lookDir;
    private int activeSpellIndex;

    public Vector3 MoveDir { get; private set; }
    public IController Controller { get; private set; }

    private IWeapon currentWeapon;
    private WeaponManager weaponManager;

    void Awake()
    {
        magicBox = GetComponentInChildren<MagicBox>();
        weaponManager = GetComponentInChildren<WeaponManager>();
    }

    private void Start()
    {
        playerMovementState = new PlayerGroundedState(this, GetComponent<Animator>());
        character = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        lookDir = transform.forward;
        activeSpellIndex = 0;
    }

    private void Update()
    {
        if (PlayerNumber == 1)
            Controller = ControllerManager.GetInstance().GetComponent<ControllerManager>().GetPlayerOneController();
        else
            Controller = ControllerManager.GetInstance().GetComponent<ControllerManager>().GetPlayerTwoController();

        if (!(Controller is NullController))
        {
            if (!(playerMovementState is PlayerDeathState))
            {
                UpdateInput();
                playerMovementState.Update();
            }
        }
    }

    private void UpdateInput()
    {
        HandleMove();
        HandleRotation();
        HandleMagicChange();

        if (Controller.GetControllerActions().rightBumper.WasPressed)
        {
                if (currentWeapon != null && !currentWeapon.CheckIfAttacking()) 
                {
                    string trigger = weaponManager.GetPrimaryAttackAnimationTrigger();
                    Debug.Log(trigger);
                    //animator.SetTrigger("MeleeTrigger");
                    animator.SetTrigger(trigger);
                    currentWeapon.SwingWeapon(animator.GetCurrentAnimatorStateInfo(1).length);
                }
        }

        if (Controller.GetControllerActions().leftBumper.WasPressed)
        {
            if (magicBox.FireMagic(activeSpellIndex))
            {
                animator.SetTrigger("CastTrigger");
            }
        }

        if (Controller.GetControllerActions().rightStickClick.WasPressed)
        {
            lockAim = !lockAim;
        }
    }

    public void ChangeCurrentWeapon(IWeapon weapon) 
    {
        currentWeapon = weapon;
    }

    public void ChangeMovementState(PlayerMovementState state)
    {
        playerMovementState = state;
    }

    public void HandleRotation()
    {
        if (!lockAim)
        {
            if(Controller.isUsingKeyboard())
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

        Vector3 moveDir = Vector3.right * Controller.GetControllerActions().move.X + Vector3.forward * Controller.GetControllerActions().move.Y;
        if (lookDir.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir, Vector3.up), RotateSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            Crosshair.SetActive(true);
        }
        else if (moveDir.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir, Vector3.up), RotateSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            Crosshair.SetActive(false);
        }

        Crosshair.transform.localPosition = new Vector3(0f, 0f, CrosshairDistance);
    }


    public void HandleMove()
    {
        MoveDir = new Vector3(Controller.GetControllerActions().move.X, 0f, Controller.GetControllerActions().move.Y);

        if (character.isGrounded)
        {
            MoveDir *= MoveSpeed;
            playerMovementState.HandleGroundedTransition();
            
            // Handle Roll Input
            if (Controller.GetControllerActions().action2.WasPressed && !IsRolling())
            {
                playerMovementState.HandleRollingTransition();
            }

            // Handle Jump Input
            verticalVelocity = -Gravity * Time.deltaTime;
            if (Controller.GetControllerActions().action1.WasPressed && !(animator.GetCurrentAnimatorStateInfo(0).IsName("Land") || animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || IsRolling()))
            {
                playerMovementState.HandleJumpingTransition();
                verticalVelocity = JumpSpeed;
            }
        }
        else
        {
            MoveDir *= MoveSpeed;
            verticalVelocity -= Gravity * Time.deltaTime;
        }

        MoveDir = new Vector3(MoveDir.x, verticalVelocity, MoveDir.z);
        character.Move(MoveDir * Time.deltaTime);
    }

    public void HandleDeath()
    {
        playerMovementState.HandleDeathTransition();
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

    private void HandleMagicChange()
    {
        ControllerActions actions = Controller.GetControllerActions();
        int totalNumberOfSpells = magicBox.GetNumberOfSpells();

        if (actions.dPadLeft.WasPressed)
        {
            activeSpellIndex = (activeSpellIndex - 1 + totalNumberOfSpells) % totalNumberOfSpells;
        }
        else if (actions.dPadRight.WasPressed)
        {
            activeSpellIndex = (activeSpellIndex + 1) % totalNumberOfSpells;
        }
    }
}
