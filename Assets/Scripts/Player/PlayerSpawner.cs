using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public bool isPlayerOne;

    void Start()
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
    }
}
