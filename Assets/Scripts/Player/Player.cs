using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMovementState playerMovementState;

    private void Start()
    {
        this.playerMovementState = new PlayerIdleState(this, GetComponent<Animator>());
    }

    private void Update()
    {
    }

    public void ChangeMovementState(PlayerMovementState state)
    {
        this.playerMovementState = state;
    }


    public void RequestMove()
    {

    }

}
