using UnityEngine;

class PlayerJumpingState : PlayerMovementState
{
    public PlayerJumpingState(Player player, Animator anim)
    {
        this.player = player;
        this.animator = anim;
        this.animator.SetBool("isJumping", true);
    }

    public override void HandleIdleTransition()
    {
        this.player.ChangeMovementState(new PlayerIdleState(this.player, this.animator));
        this.animator.SetBool("isJumping", false);
    }

    public override void HandleMovingTransition()
    {
        this.player.ChangeMovementState(new PlayerMovingState(this.player, this.animator));
        this.animator.SetBool("isJumping", false);
    }

    public override void HandleJumpingTransition()
    {
    }

    public override void Update()
    {
    }
}
