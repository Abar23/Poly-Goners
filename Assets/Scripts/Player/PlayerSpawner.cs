using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private bool playerIsSet = false;
    public bool isPlayerOne;

    void FixedUpdate()
    {
        if (this.playerIsSet == false)
        {
            GameObject playerOne = PlayerManager.GetInstance().GetPlayerOneGameObject();
            GameObject playerTwo = PlayerManager.GetInstance().GetPlayerTwoGameObject();

            if (this.isPlayerOne)
            {
                playerOne.transform.position = this.transform.position;
                playerOne.transform.rotation = this.transform.rotation;
            }
            else
            {
                playerTwo.transform.position = this.transform.position;
                playerTwo.transform.rotation = this.transform.rotation;
            }
            this.playerIsSet = true;
        }
    }
}