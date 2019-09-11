using UnityEngine;

class PlayerJumpingState : PlayerMovementState
{
    public PlayerJumpingState(Player player, Animator anim)
    {
        this.player = player;
        this.anim = anim;
        this.anim.SetBool("isJumping", true);
    }

    public override void HandleIdleTransition()
    {
        this.player.ChangeMovementState(new PlayerIdleState(this.player, this.anim));
        this.anim.SetBool("isJumping", false);
    }

    public override void HandleMovingTransition()
    {
        this.player.ChangeMovementState(new PlayerMovingState(this.player, this.anim));
        this.anim.SetBool("isJumping", false);
    }

    public override void HandleJumpingTransition()
    {
    }
}
