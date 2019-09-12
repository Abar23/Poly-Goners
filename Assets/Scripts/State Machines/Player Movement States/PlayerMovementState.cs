using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMovementState
{
    protected Player player;
    protected Animator animator;

    public abstract void HandleIdleTransition();

    public abstract void HandleMovingTransition();

    public abstract void HandleJumpingTransition();

    public abstract void Update();
}
