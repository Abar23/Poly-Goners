using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMovementState
{
    protected Player player;
    protected Animator animator;

    public abstract void HandleGroundedTransition();

    public abstract void HandleJumpingTransition();

    public abstract void HandleRollingTransition();

    public abstract void HandleDeathTransition();

    public abstract void Update();
}
