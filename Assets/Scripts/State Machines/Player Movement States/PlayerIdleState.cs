using UnityEngine;

class PlayerIdleState : PlayerMovementState
{
    public PlayerIdleState(Player player, Animator anim)
    {
        this.player = player;
        this.anim = anim;
        this.anim.SetBool("isIdle", true);
    }

    public override void HandleIdleTransition()
    {
    }

    public override void HandleMovingTransition()
    {
        this.anim.SetBool("isIdle", false);
        this.player.ChangeMovementState(new PlayerMovingState(this.player, this.anim));
    }
}
