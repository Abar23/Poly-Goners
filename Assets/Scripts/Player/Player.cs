﻿using System;
using UnityEngine;
using UnityEngine.UI;

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

    #region MagicCast
    private MagicBox magicBox;
    #endregion

    private CharacterController character;
    private Animator animator;
    private float verticalVelocity;
    private bool lockAim = false;
    private Vector3 lookDir;
    private int activeSpellIndex;
    private float reviveTimer;
    private float timeToRevive = 3f;
    private float reviveDistance = 2.5f;

    public Vector3 MoveDir { get; private set; }
    public IController Controller { get; private set; }
    public PlayerMovementState PlayerMovementState { get; private set; }

    private IWeapon currentWeapon;

    private void Awake()
    {
        magicBox = GetComponentInChildren<MagicBox>();
    }

    private void Start()
    {
        PlayerMovementState = new PlayerGroundedState(this, GetComponent<Animator>());
        RevivePrompt.SetActive(false);
        character = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        lookDir = transform.forward;
        activeSpellIndex = 0;
        reviveTimer = 0f;
    }

    private void Update()
    {
        if (PlayerNumber == 1)
            Controller = ControllerManager.GetInstance().GetComponent<ControllerManager>().GetPlayerOneController();
        else
            Controller = ControllerManager.GetInstance().GetComponent<ControllerManager>().GetPlayerTwoController();

        if (!(Controller is NullController))
        {
            if (!(PlayerMovementState is PlayerDeathState))
            {
                UpdateInput();
                PlayerMovementState.Update();
            }
        }
    }

    private void UpdateInput()
    {
        HandleMove();
        HandleRotation();
        HandleMagicChange();

        float dist = Vector3.Distance(transform.position, OtherPlayer.transform.position);

        // Check if able to revive the other player
        if (dist < reviveDistance && OtherPlayer.PlayerMovementState is PlayerDeathState)
        {
            OtherPlayer.RevivePrompt.gameObject.SetActive(true);
            OtherPlayer.RevivePromptFill.fillAmount = reviveTimer / timeToRevive;

            if (Controller.GetControllerActions().action3.IsPressed)
            {
                reviveTimer += Time.deltaTime;

                if (reviveTimer > timeToRevive)
                {
                    OtherPlayer.PlayerMovementState.HandleGroundedTransition();
                    OtherPlayer.GetComponent<Damageable>().RevivePlayer();
                }
            }
            else
            {
                if (reviveTimer > 0f)
                    reviveTimer -= Time.deltaTime * 2f;
            }
        }
        else if (dist >= reviveDistance && OtherPlayer.PlayerMovementState is PlayerDeathState)
        {
            OtherPlayer.RevivePrompt.gameObject.SetActive(false);
            reviveTimer = 0f;
        }

        // Perform melee attack
        if (Controller.GetControllerActions().rightBumper.WasPressed)
        {
            if (currentWeapon != null && !currentWeapon.CheckIfAttacking()) 
            {
                animator.SetTrigger("MeleeTrigger");
                currentWeapon.SwingWeapon(animator.GetCurrentAnimatorStateInfo(1).length);
            }
        }

        // Perform magic attack
        if (Controller.GetControllerActions().leftBumper.WasPressed)
        {
            if (magicBox.FireMagic(activeSpellIndex))
            {
                animator.SetTrigger("CastTrigger");
            }
        }

        // Lock on 
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
        PlayerMovementState = state;
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
            PlayerMovementState.HandleGroundedTransition();
            
            // Handle Roll Input
            if (Controller.GetControllerActions().action2.WasPressed && !IsRolling())
            {
                PlayerMovementState.HandleRollingTransition();
            }

            // Handle Jump Input
            verticalVelocity = -Gravity * Time.deltaTime;
            if (Controller.GetControllerActions().action1.WasPressed && !(animator.GetCurrentAnimatorStateInfo(0).IsName("Land") || animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || IsRolling()))
            {
                PlayerMovementState.HandleJumpingTransition();
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
        PlayerMovementState.HandleDeathTransition();
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
