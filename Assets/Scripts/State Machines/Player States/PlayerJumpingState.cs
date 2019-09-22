using UnityEngine;

class PlayerJumpingState : PlayerMovementState
{
    public PlayerJumpingState(Player player, Animator anim)
    {
        this.player = player;
        animator = anim;
        animator.SetBool("isJumping", true);
    }

    public override void HandleGroundedTransition()
    {
        animator.SetBool("isJumping", false);
        player.ChangeMovementState(new PlayerGroundedState(this.player, this.animator));
    }

    public override void HandleJumpingTransition()
    {
    }
    public override void HandleRollingTransition()
    {
        animator.SetBool("isJumping", false);
        player.ChangeMovementState(new PlayerRollingState(this.player, this.animator));
    }

    public override void HandleDeathTransition()
    {
        animator.SetBool("isJumping", false);
        player.ChangeMovementState(new PlayerDeathState(this.player, this.animator));
    }

    public override void Update()
    {
    }
}
