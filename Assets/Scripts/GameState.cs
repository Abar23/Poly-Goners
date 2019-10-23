using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private PlayerManager playerManager;
    private bool playerOneDead;
    private bool playerTwoDead;
    private bool gameOverMenuDisplayed;

    void Start() {
        playerManager = PlayerManager.GetInstance();
        playerOneDead = false;
        playerTwoDead = false;
        gameOverMenuDisplayed = false;
    }

    void Update() {
        if (!gameOverMenuDisplayed && CheckIfAllPlayersDead()) {
            GetComponentInChildren<GameOverMenuManager>().DisplayGameOverMenu();
            gameOverMenuDisplayed = true;
        }
    }

    private bool CheckIfAllPlayersDead() {
        int numberPlayers = playerManager.GetNumberOfActivePlayers();
        bool allPlayersDead = false;

        if (numberPlayers == 1 && playerManager.GetPlayerOneGameObject().GetComponent<Player>().PlayerMovementState is PlayerDeathState) {
            playerOneDead = true;
            allPlayersDead = true;
        }

        if (numberPlayers == 2 && playerManager.GetPlayerTwoGameObject().GetComponent<Player>().PlayerMovementState is PlayerDeathState) {
            playerTwoDead = true;

            if (playerOneDead)
                allPlayersDead = true;
        }

        return allPlayersDead;
    }

    public void RevivePlayers() {
        Player playerOne = playerManager.GetPlayerOneGameObject().GetComponent<Player>();
        Player playerTwo = playerManager.GetPlayerTwoGameObject().GetComponent<Player>();

        playerOne.PlayerMovementState.HandleGroundedTransition();
        playerOne.OnRevive();
        playerOne.GetComponent<Damageable>().ResetHealthToFull();
        playerOne.GetComponentInChildren<MagicBox>().ResetMagicToFull();
        playerOne.GetComponent<Stamina>().ResetStaminaToFull();
        playerOne.GetComponent<Inventory>().OnDeath();

        if (playerManager.GetNumberOfActivePlayers() == 2) {
            playerTwo.PlayerMovementState.HandleGroundedTransition();
            playerTwo.OnRevive();
            playerTwo.GetComponent<Damageable>().ResetHealthToFull();
            playerTwo.GetComponentInChildren<MagicBox>().ResetMagicToFull();
            playerTwo.GetComponent<Stamina>().ResetStaminaToFull();
            playerTwo.GetComponent<Inventory>().OnDeath();
        }
    }
}

