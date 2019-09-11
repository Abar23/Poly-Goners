using UnityEngine;

class PlayerMovingState : PlayerMovementState
{
    public PlayerMovingState(Player player, Animator anim)
    {
        this.player = player;
        this.anim = anim;
    }

    public override void HandleIdleTransition()
    {
        this.anim.SetBool("isRunningForward", false);
        this.anim.SetBool("isRunningBackward", false);
        this.anim.SetBool("isStrafingLeft", false);
        this.anim.SetBool("isStrafingRight", false);
        this.anim.SetBool("isRunningBackLeft", false);
        this.anim.SetBool("isRunningBackRight", false);
        this.anim.SetBool("isRunningForwardLeft", false);
        this.anim.SetBool("isRunningForwardRight", false);
        this.player.ChangeMovementState(new PlayerIdleState(this.player, this.anim));
    }

    public override void HandleMovingTransition()
    {
    }

    public override void HandleJumpingTransition()
    {
        this.anim.SetBool("isRunningForward", false);
        this.anim.SetBool("isRunningBackward", false);
        this.anim.SetBool("isStrafingLeft", false);
        this.anim.SetBool("isStrafingRight", false);
        this.anim.SetBool("isRunningBackLeft", false);
        this.anim.SetBool("isRunningBackRight", false);
        this.anim.SetBool("isRunningForwardLeft", false);
        this.anim.SetBool("isRunningForwardRight", false);
        this.player.ChangeMovementState(new PlayerJumpingState(this.player, this.anim));
    }
}
