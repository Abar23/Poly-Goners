using UnityEngine;

class PlayerJumpingState : PlayerMovementState
{
    public PlayerJumpingState(Player player, Animator anim)
    {
        this.player = player;
        this.animator = anim;
        this.animator.SetBool("isJumping", true);
    }

    public override void HandleGroundedTransition()
    {
        this.player.ChangeMovementState(new PlayerGroundedState(this.player, this.animator));
        this.animator.SetBool("isJumping", false);
    }

    public override void HandleJumpingTransition()
    {
    }

    public override void Update()
    {
    }
}
