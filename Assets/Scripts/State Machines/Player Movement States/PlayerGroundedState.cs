using UnityEngine;

class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(Player player, Animator anim)
    {
        this.player = player;
        animator = anim;
        animator.SetBool("isMoving", true);
    }

    public override void HandleGroundedTransition()
    {
    }

    public override void HandleJumpingTransition()
    {
        animator.SetBool("isMoving", false);
        player.ChangeMovementState(new PlayerJumpingState(player, animator));
    }

    public override void Update()
    {
        float angleBetween = Vector3.Angle(player.transform.forward, player.MoveDir);
        HandleRollInputs(angleBetween);


        if (angleBetween < 22.5f)
        {
            animator.SetFloat("HorizontalMovement", 0f, 1f, Time.smoothDeltaTime * 10f);
            animator.SetFloat("VerticalMovement", player.MoveDir.normalized.magnitude, 1f, Time.smoothDeltaTime * 10f);
        }
        else
        {
            if (player.transform.forward.z > 0 && Mathf.Abs(player.transform.forward.z) > 0.5) // facing north
            {
                animator.SetFloat("HorizontalMovement", player.MoveDir.normalized.x, 1f, Time.smoothDeltaTime * 10f);
                animator.SetFloat("VerticalMovement", player.MoveDir.normalized.z, 1f, Time.smoothDeltaTime * 10f);
            }
            else if (player.transform.forward.z < 0 && Mathf.Abs(player.transform.forward.z) > 0.5) // facing south
            {
                animator.SetFloat("HorizontalMovement", -player.MoveDir.normalized.x, 1f, Time.smoothDeltaTime * 10f);
                animator.SetFloat("VerticalMovement", -player.MoveDir.normalized.z, 1f, Time.smoothDeltaTime * 10f);
            }
            else if (player.transform.forward.x > 0 && Mathf.Abs(player.transform.forward.x) > 0.5) // facing east
            {
                animator.SetFloat("HorizontalMovement", -player.MoveDir.normalized.z, 1f, Time.smoothDeltaTime * 10f);
                animator.SetFloat("VerticalMovement", player.MoveDir.normalized.x, 1f, Time.smoothDeltaTime * 10f);
            }
            else if (player.transform.forward.x < 0 && Mathf.Abs(player.transform.forward.x) > 0.5) // facing west
            {
                animator.SetFloat("HorizontalMovement", player.MoveDir.normalized.z, 1f, Time.smoothDeltaTime * 10f);
                animator.SetFloat("VerticalMovement", -player.MoveDir.normalized.x, 1f, Time.smoothDeltaTime * 10f);
            }
        }




        if (angleBetween >= 22.5 && angleBetween < 67.5 && player.MoveDir.x < 0 && player.transform.forward.z >= 0)
        {
            animator.SetBool("isRunningForwardLeft", true);
        }
        else if (angleBetween >= 22.5 && angleBetween < 67.5 && player.MoveDir.x > 0 && player.transform.forward.z >= 0)
        {
            animator.SetBool("isRunningForwardRight", true);
        }
        else if (angleBetween >= 22.5 && angleBetween < 67.5 && player.MoveDir.x < 0 && player.transform.forward.z < 0)
        {
            animator.SetBool("isRunningForwardRight", true);
        }
        else if (angleBetween >= 22.5 && angleBetween < 67.5 && player.MoveDir.x > 0 && player.transform.forward.z < 0)
        {
            animator.SetBool("isRunningForwardLeft", true);
        }
        else if (angleBetween >= 67.5 && angleBetween < 112.5 && player.MoveDir.x < 0 && player.transform.forward.z >= 0)
        {
            animator.SetBool("isStrafingLeft", true);
        }
        else if (angleBetween >= 67.5 && angleBetween < 112.5 && player.MoveDir.x > 0 && player.transform.forward.z >= 0)
        {
            animator.SetBool("isStrafingRight", true);
        }
        else if (angleBetween >= 67.5 && angleBetween < 112.5 && player.MoveDir.x < 0 && player.transform.forward.z < 0)
        {
            animator.SetBool("isStrafingRight", true);
        }
        else if (angleBetween >= 67.5 && angleBetween < 112.5 && player.MoveDir.x > 0 && player.transform.forward.z < 0)
        {
            animator.SetBool("isStrafingLeft", true);
        }
        else if (angleBetween >= 112.5 && angleBetween < 157.5 && player.MoveDir.x < 0 && player.transform.forward.z >= 0)
        {
            animator.SetBool("isRunningBackLeft", true);
        }
        else if (angleBetween >= 157.5)
        {
            animator.SetBool("isRunningBackward", true);
        }
    }

    private void HandleRollInputs(float angleBetween)
    {
        // action2 == Roll input
        if (player.Controller.GetControllerActions().action2.WasPressed)
        {
            Debug.Log(player.MoveDir);

            // Standing Idle
            if (player.MoveDir.x == 0f && player.MoveDir.z == 0f)
            {
                animator.SetTrigger("RollBack");
            }
            // Running forward
            else if (angleBetween <= 45)
            {
                animator.SetTrigger("RollForward");
            }
            // Running backwards
            else if (angleBetween >= 135)
            {
                animator.SetTrigger("RollBack");
            }
            // Strafing
            else if (angleBetween > 45 && angleBetween < 135)
            {
                if (player.transform.forward.z > 0 && Mathf.Abs(player.transform.forward.z) > 0.5) // facing north
                {
                    if (player.MoveDir.x < 0)
                        animator.SetTrigger("RollLeft");
                    else
                        animator.SetTrigger("RollRight");
                }
                else if (player.transform.forward.z < 0 && Mathf.Abs(player.transform.forward.z) > 0.5) // facing south
                {
                    if (player.MoveDir.x < 0)
                        animator.SetTrigger("RollRight");
                    else
                        animator.SetTrigger("RollLeft");
                }
                else if (player.transform.forward.x > 0 && Mathf.Abs(player.transform.forward.x) > 0.5) // facing east
                {
                    if (player.MoveDir.z < 0)
                        animator.SetTrigger("RollRight");
                    else
                        animator.SetTrigger("RollLeft");
                }
                else if (player.transform.forward.x < 0 && Mathf.Abs(player.transform.forward.x) > 0.5) // facing west
                {
                    if (player.MoveDir.z < 0)
                        animator.SetTrigger("RollLeft");
                    else
                        animator.SetTrigger("RollRight");
                }
            }
        }
    }
}
