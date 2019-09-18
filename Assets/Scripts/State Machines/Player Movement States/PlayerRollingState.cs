using UnityEngine;

class PlayerRollingState : PlayerMovementState
{
    public PlayerRollingState(Player player, Animator anim)
    {
        this.player = player;
        animator = anim;
        animator.SetBool("isRolling", true);
    }

    public override void HandleGroundedTransition()
    {
        animator.SetBool("isRolling", false);
        player.ChangeMovementState(new PlayerGroundedState(this.player, this.animator));
    }

    public override void HandleRollingTransition()
    {
    }

    public override void HandleJumpingTransition()
    {
    }

    public override void Update()
    {
        float angleBetween = Vector3.Angle(player.transform.forward, player.MoveDir);

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
