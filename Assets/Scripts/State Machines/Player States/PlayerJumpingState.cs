using UnityEngine;

class PlayerJumpingState : PlayerMovementState
{
    public PlayerJumpingState(Player player, Animator anim, bool isJumping)
    {
        this.player = player;
        animator = anim;
        animator.SetBool("isJumping", isJumping);
        
    }

    public override void HandleGroundedTransition()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("Fall", true);
        player.ChangeMovementState(new PlayerGroundedState(this.player, this.animator));
    }

    public override void HandleJumpingTransition()
    {
    }
    public override void HandleRollingTransition()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("Fall", true);
        player.ChangeMovementState(new PlayerRollingState(this.player, this.animator));
    }

    public override void HandleDeathTransition()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("Fall", true);
        player.ChangeMovementState(new PlayerDeathState(this.player, this.animator));
    }

    public override void Update()
    {
        
        RaycastHit hit;
        Vector3 offset = new Vector3(0, 0.1f, 0);
        if (player.VerticalVelocity < 0)
        {
            if (Physics.Raycast((player.transform.position + offset), -Vector3.up, out hit, 100f))
            {
                float distanceToGround = hit.distance;
                if (distanceToGround < 0.5f)
                {
                    animator.SetBool("Fall", false);
                }
            }
        }
    }
}
