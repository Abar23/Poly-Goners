using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MoveSpeed = 1.0f;
    public float RotateSpeed = 1.0f;
    public float Gravity = 14.0f;
    public float JumpSpeed = 10.0f;
    public float CrosshairDistance = 3.0f;
    public GameObject Crosshair;

    private PlayerMovementState playerMovementState;
    private ControllerManager controllerManager;
    private CharacterController characterController;
    private Animator anim;
    private float verticalVelocity;
    private bool lockAim = false;
    private Vector3 lookDir;

    private void Start()
    {
        playerMovementState = new PlayerIdleState(this, GetComponent<Animator>());
        controllerManager = ControllerManager.GetInstance().GetComponent<ControllerManager>();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        lookDir = transform.forward;
    }

    private void Update()
    {
        RequestMove();
        RequestRotation();

        if (controllerManager.GetPlayerOneController().GetControllerActions().rightBumper.WasPressed)
        {
            anim.SetTrigger("MeleeTrigger");
        }

        if (controllerManager.GetPlayerOneController().GetControllerActions().leftBumper.WasPressed)
        {
            anim.SetTrigger("CastTrigger");
        }   

        if (controllerManager.GetPlayerOneController().GetControllerActions().action2.WasPressed &&  playerMovementState is PlayerIdleState)
        {
            anim.SetTrigger("RollBack");
        }

        if (controllerManager.GetPlayerOneController().GetControllerActions().rightStickClick.WasPressed)
        {
            lockAim = !lockAim;
        }
    }

    public void ChangeMovementState(PlayerMovementState state)
    {
        playerMovementState = state;
    }

    public void RequestRotation()
    {
        if (!lockAim)
        {
            lookDir = Vector3.right * controllerManager.GetPlayerOneController().GetControllerActions().look.X + Vector3.forward * controllerManager.GetPlayerOneController().GetControllerActions().look.Y;
        }

        Vector3 moveDir = Vector3.right * controllerManager.GetPlayerOneController().GetControllerActions().move.X + Vector3.forward * controllerManager.GetPlayerOneController().GetControllerActions().move.Y;
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


    public void RequestMove()
    {
        Vector3 moveDir = new Vector3(controllerManager.GetPlayerOneController().GetControllerActions().move.X, 0f, controllerManager.GetPlayerOneController().GetControllerActions().move.Y);

        if (characterController.isGrounded)
        {
            if (moveDir.Equals(Vector3.zero))
            {
                playerMovementState.HandleIdleTransition();
            }
            else
            {
                playerMovementState.HandleMovingTransition();
                moveDir *= MoveSpeed;
                UpdateMovingAnimation(moveDir);
            }
            

            verticalVelocity = -Gravity * Time.deltaTime;
            if (controllerManager.GetPlayerOneController().GetControllerActions().action1.WasPressed && !(playerMovementState is PlayerJumpingState))
            {
                playerMovementState.HandleJumpingTransition();
                verticalVelocity = JumpSpeed;
            }
        }
        else
        {
            moveDir *= MoveSpeed;
            verticalVelocity -= Gravity * Time.deltaTime;
        }

        moveDir.y = verticalVelocity;
        characterController.Move(moveDir * Time.deltaTime);
        anim.SetFloat("VerticalVelocity", verticalVelocity);

        float angleBetween = Vector3.Angle(transform.forward, moveDir);

        if (angleBetween > 45 && angleBetween < 135 && moveDir.x < 0 && controllerManager.GetPlayerOneController().GetControllerActions().action2.WasPressed && playerMovementState is PlayerMovingState && transform.forward.z >= 0)
        {
            anim.SetTrigger("RollLeft");
        }
        if (angleBetween > 45 && angleBetween < 135 && moveDir.x > 0 && controllerManager.GetPlayerOneController().GetControllerActions().action2.WasPressed && playerMovementState is PlayerMovingState && transform.forward.z >= 0)
        {
            anim.SetTrigger("RollRight");
        }
        if (angleBetween > 45 && angleBetween < 135 && moveDir.x < 0 && controllerManager.GetPlayerOneController().GetControllerActions().action2.WasPressed && playerMovementState is PlayerMovingState && transform.forward.z < 0)
        {
            anim.SetTrigger("RollRight");
        }
        if (angleBetween > 45 && angleBetween < 135 && moveDir.x > 0 && controllerManager.GetPlayerOneController().GetControllerActions().action2.WasPressed && playerMovementState is PlayerMovingState && transform.forward.z < 0)
        {
            anim.SetTrigger("RollLeft");
        }
        if (angleBetween <= 45f && controllerManager.GetPlayerOneController().GetControllerActions().action2.WasPressed && playerMovementState is PlayerMovingState)
        {
            anim.SetTrigger("RollForward");
        }
        if (angleBetween >= 135 && controllerManager.GetPlayerOneController().GetControllerActions().action2.WasPressed && playerMovementState is PlayerMovingState)
        {
            anim.SetTrigger("RollBack");
        }
    }

    private void UpdateMovingAnimation(Vector3 moveDir)
    {
        if (playerMovementState is PlayerMovingState)
        {
            float angleBetween = Vector3.Angle(transform.forward, moveDir);
            if (angleBetween < 22.5)
            {
                DisableAnimations();
                anim.SetBool("isRunningForward", true);
            }
            else if(angleBetween >= 22.5 && angleBetween < 67.5 && moveDir.x < 0 && transform.forward.z >= 0)
            {
                DisableAnimations();
                anim.SetBool("isRunningForwardLeft", true);
            }
            else if (angleBetween >= 22.5 && angleBetween < 67.5 && moveDir.x > 0 && transform.forward.z >= 0)
            {
                DisableAnimations();
                anim.SetBool("isRunningForwardRight", true);
            }
            else if (angleBetween >= 22.5 && angleBetween < 67.5 && moveDir.x < 0 && transform.forward.z < 0)
            {
                DisableAnimations();
                anim.SetBool("isRunningForwardRight", true);
            }
            else if (angleBetween >= 22.5 && angleBetween < 67.5 && moveDir.x > 0 && transform.forward.z < 0)
            {
                DisableAnimations();
                anim.SetBool("isRunningForwardLeft", true);
            }
            else if (angleBetween >= 67.5 && angleBetween < 112.5 && moveDir.x < 0 && transform.forward.z >= 0)
            {
                DisableAnimations();
                anim.SetBool("isStrafingLeft", true);
            }
            else if (angleBetween >= 67.5 && angleBetween < 112.5 && moveDir.x > 0 && transform.forward.z >= 0)
            {
                DisableAnimations();
                anim.SetBool("isStrafingRight", true);
            }
            else if (angleBetween >= 67.5 && angleBetween < 112.5 && moveDir.x < 0 && transform.forward.z < 0)
            {
                DisableAnimations();
                anim.SetBool("isStrafingRight", true);
            }
            else if (angleBetween >= 67.5 && angleBetween < 112.5 && moveDir.x > 0 && transform.forward.z < 0)
            {
                DisableAnimations();
                anim.SetBool("isStrafingLeft", true);
            }
            else if (angleBetween >= 112.5 && angleBetween < 157.5 && moveDir.x < 0 && transform.forward.z >= 0)
            {
                DisableAnimations();
                anim.SetBool("isRunningBackLeft", true);
            }
            else if (angleBetween >= 112.5 && angleBetween < 157.5 && moveDir.x > 0 && transform.forward.z >= 0)
            {
                DisableAnimations();
                anim.SetBool("isRunningBackRight", true);
            }
            else if (angleBetween >= 112.5 && angleBetween < 157.5 && moveDir.x < 0 && transform.forward.z < 0)
            {
                DisableAnimations();
                anim.SetBool("isRunningBackRight", true);
            }
            else if (angleBetween >= 112.5 && angleBetween < 157.5 && moveDir.x > 0 && transform.forward.z < 0)
            {
                DisableAnimations();
                anim.SetBool("isRunningBackLeft", true);
            }
            else if (angleBetween >= 157.5)
            {
                DisableAnimations();
                anim.SetBool("isRunningBackward", true);
            }
        }
    }

    private void DisableAnimations()
    {
        anim.SetBool("isRunningForward", false);
        anim.SetBool("isRunningBackward", false);
        anim.SetBool("isStrafingLeft", false);
        anim.SetBool("isStrafingRight", false);
        anim.SetBool("isRunningBackLeft", false);
        anim.SetBool("isRunningBackRight", false);
        anim.SetBool("isRunningForwardLeft", false);
        anim.SetBool("isRunningForwardRight", false);
    }

}
