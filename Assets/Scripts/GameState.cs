﻿using System.Collections;
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
        bool allPlayersDead = false;
        int numberPlayers = playerManager.GetNumberOfActivePlayers();
        Player playerOne = playerManager.GetPlayerOneGameObject().GetComponent<Player>();
        Player playerTwo = playerManager.GetPlayerTwoGameObject().GetComponent<Player>();

        if (numberPlayers == 1)
        {
            if(playerOne.IsDead())
            {
                allPlayersDead = true;
            }
        }
        else if (numberPlayers == 2)
        {
            if(playerOne.IsDead() && playerTwo.IsDead())
            {
                allPlayersDead = true;
            }
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

