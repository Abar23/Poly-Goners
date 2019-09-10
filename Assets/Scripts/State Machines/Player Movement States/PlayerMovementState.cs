using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMovementState
{
    protected Player player;
    protected Animator anim;

    public abstract void HandleIdleTransition();

    public abstract void HandleMovingTransition();
}
