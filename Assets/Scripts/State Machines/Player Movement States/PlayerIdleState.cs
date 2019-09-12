using UnityEngine;

class PlayerIdleState : PlayerMovementState
{
    public PlayerIdleState(Player player, Animator anim)
    {
        this.player = player;
        this.animator = anim;
        this.animator.SetBool("isIdle", true);
    }

    public override void HandleIdleTransition()
    {
    }

    public override void HandleMovingTransition()
    {
        this.animator.SetBool("isIdle", false);
        this.player.ChangeMovementState(new PlayerMovingState(this.player, this.animator));
    }

    public override void HandleJumpingTransition()
    {
        this.animator.SetBool("isIdle", false);
        this.player.ChangeMovementState(new PlayerJumpingState(this.player, this.animator));
    }

    public override void Update()
    {
        if (player.Controller.GetControllerActions().action2.WasPressed)
        {
            animator.SetTrigger("RollBack");
        }
    }
}
