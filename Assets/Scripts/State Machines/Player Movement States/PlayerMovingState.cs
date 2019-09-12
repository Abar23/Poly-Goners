using UnityEngine;

class PlayerMovingState : PlayerMovementState
{
    public PlayerMovingState(Player player, Animator anim)
    {
        this.player = player;
        animator = anim;
        UpdateMovingAnimation();
    }

    public override void HandleIdleTransition()
    {
        DisableAnimations();
        player.ChangeMovementState(new PlayerIdleState(player, animator));
    }

    public override void HandleMovingTransition()
    {
    }

    public override void HandleJumpingTransition()
    {
        DisableAnimations();
        player.ChangeMovementState(new PlayerJumpingState(player, animator));
    }

    public override void Update()
    {
        UpdateMovingAnimation();

        float angleBetween = Vector3.Angle(player.transform.forward, player.MoveDir);

        // Strafing
        if (angleBetween > 45 && angleBetween < 135 && player.Controller.GetControllerActions().action2.WasPressed)
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

        // Running forward
        if (angleBetween <= 45f && player.Controller.GetControllerActions().action2.WasPressed)
        {
            animator.SetTrigger("RollForward");
        }
        // Running backwards
        if (angleBetween >= 135 && player.Controller.GetControllerActions().action2.WasPressed)
        {
            animator.SetTrigger("RollBack");
        }

    }

    private void UpdateMovingAnimation()
    {
        float angleBetween = Vector3.Angle(player.transform.forward, player.MoveDir);

        if (angleBetween < 22.5)
        {
            DisableAnimations();
            animator.SetBool("isRunningForward", true);
        }
        else if (angleBetween >= 22.5 && angleBetween < 67.5 && player.MoveDir.x < 0 && player.transform.forward.z >= 0)
        {
            DisableAnimations();
            animator.SetBool("isRunningForwardLeft", true);
        }
        else if (angleBetween >= 22.5 && angleBetween < 67.5 && player.MoveDir.x > 0 && player.transform.forward.z >= 0)
        {
            DisableAnimations();
            animator.SetBool("isRunningForwardRight", true);
        }
        else if (angleBetween >= 22.5 && angleBetween < 67.5 && player.MoveDir.x < 0 && player.transform.forward.z < 0)
        {
            DisableAnimations();
            animator.SetBool("isRunningForwardRight", true);
        }
        else if (angleBetween >= 22.5 && angleBetween < 67.5 && player.MoveDir.x > 0 && player.transform.forward.z < 0)
        {
            DisableAnimations();
            animator.SetBool("isRunningForwardLeft", true);
        }
        else if (angleBetween >= 67.5 && angleBetween < 112.5 && player.MoveDir.x < 0 && player.transform.forward.z >= 0)
        {
            DisableAnimations();
            animator.SetBool("isStrafingLeft", true);
        }
        else if (angleBetween >= 67.5 && angleBetween < 112.5 && player.MoveDir.x > 0 && player.transform.forward.z >= 0)
        {
            DisableAnimations();
            animator.SetBool("isStrafingRight", true);
        }
        else if (angleBetween >= 67.5 && angleBetween < 112.5 && player.MoveDir.x < 0 && player.transform.forward.z < 0)
        {
            DisableAnimations();
            animator.SetBool("isStrafingRight", true);
        }
        else if (angleBetween >= 67.5 && angleBetween < 112.5 && player.MoveDir.x > 0 && player.transform.forward.z < 0)
        {
            DisableAnimations();
            animator.SetBool("isStrafingLeft", true);
        }
        else if (angleBetween >= 112.5 && angleBetween < 157.5 && player.MoveDir.x < 0 && player.transform.forward.z >= 0)
        {
            DisableAnimations();
            animator.SetBool("isRunningBackLeft", true);
        }
        else if (angleBetween >= 112.5 && angleBetween < 157.5 && player.MoveDir.x > 0 && player.transform.forward.z >= 0)
        {
            DisableAnimations();
            animator.SetBool("isRunningBackRight", true);
        }
        else if (angleBetween >= 112.5 && angleBetween < 157.5 && player.MoveDir.x < 0 && player.transform.forward.z < 0)
        {
            DisableAnimations();
            animator.SetBool("isRunningBackRight", true);
        }
        else if (angleBetween >= 112.5 && angleBetween < 157.5 && player.MoveDir.x > 0 && player.transform.forward.z < 0)
        {
            DisableAnimations();
            animator.SetBool("isRunningBackLeft", true);
        }
        else if (angleBetween >= 157.5)
        {
            DisableAnimations();
            animator.SetBool("isRunningBackward", true);
        }
    }


    private void DisableAnimations()
    {
        animator.SetBool("isRunningForward", false);
        animator.SetBool("isRunningBackward", false);
        animator.SetBool("isStrafingLeft", false);
        animator.SetBool("isStrafingRight", false);
        animator.SetBool("isRunningBackLeft", false);
        animator.SetBool("isRunningBackRight", false);
        animator.SetBool("isRunningForwardLeft", false);
        animator.SetBool("isRunningForwardRight", false);
    }
}
