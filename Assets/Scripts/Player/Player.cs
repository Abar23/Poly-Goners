using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MoveSpeed = 1.0f;
    public float RotateSpeed = 1.0f;
    private PlayerMovementState playerMovementState;
    private ControllerManager controllerManager;
    private CharacterController characterController;
    private Animator anim;

    private void Start()
    {
        playerMovementState = new PlayerIdleState(this, GetComponent<Animator>());
        controllerManager = ControllerManager.GetInstance().GetComponent<ControllerManager>();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();        
    }

    private void Update()
    {
        RequestMove();
        RequestRotation();
    }

    public void ChangeMovementState(PlayerMovementState state)
    {
        playerMovementState = state;
    }

    public void RequestRotation()
    {

        Vector3 lookDir = Vector3.right * controllerManager.GetPlayerOneController().GetControllerActions().look.X + Vector3.forward * controllerManager.GetPlayerOneController().GetControllerActions().look.Y;
        Vector3 moveDir = Vector3.right * controllerManager.GetPlayerOneController().GetControllerActions().move.X + Vector3.forward * controllerManager.GetPlayerOneController().GetControllerActions().move.Y;
        if (lookDir.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir, Vector3.up), RotateSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        else if (moveDir.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir, Vector3.up), RotateSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }


    }


    public void RequestMove()
    {
        Vector3 moveDir = new Vector3(controllerManager.GetPlayerOneController().GetControllerActions().move.X, 0f, controllerManager.GetPlayerOneController().GetControllerActions().move.Y);
        if (moveDir.Equals(Vector3.zero))
        {
            playerMovementState.HandleIdleTransition();
            anim.speed = 1;
        }
        else
        {
            playerMovementState.HandleMovingTransition();
            moveDir *= MoveSpeed;
            UpdateMovingAnimation(moveDir);
            if (moveDir.magnitude > 1)
            {
                anim.speed = 1;
            }
            else
            {
                anim.speed = (float)Math.Sqrt(moveDir.magnitude);
            }

            characterController.Move(moveDir * Time.deltaTime);
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
            else if(angleBetween >= 22.5 && angleBetween < 67.5 && moveDir.x < 0)
            {
                DisableAnimations();
                anim.SetBool("isRunningForwardLeft", true);
            }
            else if (angleBetween >= 22.5 && angleBetween < 67.5 && moveDir.x > 0)
            {
                DisableAnimations();
                anim.SetBool("isRunningForwardRight", true);
            }
            else if (angleBetween >= 67.5 && angleBetween < 112.5 && moveDir.x < 0)
            {
                DisableAnimations();
                anim.SetBool("isStrafingLeft", true);
            }
            else if (angleBetween >= 67.5 && angleBetween < 112.5 && moveDir.x > 0)
            {
                DisableAnimations();
                anim.SetBool("isStrafingRight", true);
            }
            else if (angleBetween >= 112.5 && angleBetween < 157.5 && moveDir.x < 0)
            {
                DisableAnimations();
                anim.SetBool("isRunningBackLeft", true);
            }
            else if (angleBetween >= 112.5 && angleBetween < 157.5 && moveDir.x > 0)
            {
                DisableAnimations();
                anim.SetBool("isRunningBackRight", true);
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
