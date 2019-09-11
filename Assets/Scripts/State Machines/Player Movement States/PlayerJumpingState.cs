using UnityEngine;

class PlayerJumpingState : PlayerMovementState
{
    public PlayerJumpingState(Player player, Animator anim)
    {
        this.player = player;
        this.anim = anim;
        this.anim.SetTrigger("JumpTrigger");
    }

    public override void HandleIdleTransition()
    {
        this.player.ChangeMovementState(new PlayerIdleState(this.player, this.anim));
    }

    public override void HandleMovingTransition()
    {
        this.player.ChangeMovementState(new PlayerMovingState(this.player, this.anim));
    }

    public override void HandleJumpingTransition()
    {
    }
}
