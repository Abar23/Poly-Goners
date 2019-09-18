﻿using UnityEngine;

class PlayerDeathState : PlayerMovementState
{
    public PlayerDeathState(Player player, Animator anim)
    {
        this.player = player;
        animator = anim;
        animator.SetBool("isDead", true);
    }

    public override void HandleGroundedTransition()
    {
    }

    public override void HandleJumpingTransition()
    {
    }

    public override void HandleRollingTransition()
    {
    }

    public override void HandleDeathTransition()
    {
    }

    public override void Update()
    {
    }
}
