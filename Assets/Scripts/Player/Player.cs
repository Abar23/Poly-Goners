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

    private PlayerMovementState playerMovementState;
    private CharacterController character;
    private Animator animator;
    private float verticalVelocity;
    private bool lockAim = false;
    private Vector3 lookDir;

    public Vector3 MoveDir { get; private set; }
    public IController Controller { get; private set; }

    private void Start()
    {
        playerMovementState = new PlayerIdleState(this, GetComponent<Animator>());
        character = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        lookDir = transform.forward;
    }

    private void Update()
    {
        if (PlayerNumber == 1)
            Controller = ControllerManager.GetInstance().GetComponent<ControllerManager>().GetPlayerOneController();
        else
            Controller = ControllerManager.GetInstance().GetComponent<ControllerManager>().GetPlayerTwoController();

        if (!(Controller is NullController))
        {
            UpdateInput();
            playerMovementState.Update();
        }
    }

    private void UpdateInput()
    {
        HandleMove();
        HandleRotation();

        if (Controller.GetControllerActions().rightBumper.WasPressed)
        {
            animator.SetTrigger("MeleeTrigger");
        }

        if (Controller.GetControllerActions().leftBumper.WasPressed)
        {
            animator.SetTrigger("CastTrigger");
        }

        if (Controller.GetControllerActions().rightStickClick.WasPressed)
        {
            lockAim = !lockAim;
        }
    }


    public void ChangeMovementState(PlayerMovementState state)
    {
        playerMovementState = state;
    }

    public void HandleRotation()
    {
        if (!lockAim)
        {
            lookDir = Vector3.right * Controller.GetControllerActions().look.X + Vector3.forward * Controller.GetControllerActions().look.Y;
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
            if (MoveDir.Equals(Vector3.zero))
            {
                playerMovementState.HandleIdleTransition();
            }
            else
            {
                MoveDir *= MoveSpeed;
                playerMovementState.HandleMovingTransition();
            }

            // Handle Jump Input
            verticalVelocity = -Gravity * Time.deltaTime;
            if (Controller.GetControllerActions().action1.WasPressed && !(playerMovementState is PlayerJumpingState))
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

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "MainCamera")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
