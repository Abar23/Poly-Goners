using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    //public GameObject playerHud;

    public GameObject pauseMenu;
    public GameObject MainPausePanel;
    public GameObject MainPauseText;
    public List<GameObject> OtherPanels;

    public static bool isGamePaused;

    private ControllerManager cm;
    private Player player1;
    private Player player2;

    void Start()
    {
        isGamePaused = false;
        //this.playerHud.SetActive(true);
        this.pauseMenu.SetActive(false);
        cm = ControllerManager.GetInstance();
        player1 = PlayerManager.GetInstance().GetPlayerOneGameObject().GetComponent<Player>();
        player2 = PlayerManager.GetInstance().GetPlayerTwoGameObject().GetComponent<Player>();
    }

    void LateUpdate()
    {
        bool isStartPressed = false;
        IController playerOneController = cm.GetPlayerOneController();
        IController playerTwoController = cm.GetPlayerTwoController();

        if (playerOneController is Controller)
        {
            isStartPressed = playerOneController.GetControllerActions().start.WasPressed;
        }

        if(!isStartPressed && playerTwoController is Controller)
        {
            isStartPressed = playerTwoController.GetControllerActions().start.WasPressed;
        }

        if(isStartPressed)
        {
            if(!isGamePaused)
            {
                isGamePaused = true;
                player1.SetIsPaused(true);
                player2.SetIsPaused(true);
                //this.playerHud.SetActive(false);
                this.pauseMenu.SetActive(true);
                MainPausePanel.SetActive(true);
                MainPauseText.SetActive(true);
                foreach (GameObject panel in OtherPanels)
                {
                    panel.SetActive(false);
                }
                Time.timeScale = 0.0f;
            }
            else
            {
                MainPausePanel.GetComponent<SelectOnInput>().Disable();
                foreach (GameObject panel in OtherPanels)
                {
                    if (panel.GetComponent<SelectOnInput>() != null)
                        panel.GetComponent<SelectOnInput>().Disable();
                }
                isGamePaused = false;
                player1.SetIsPaused(false);
                player2.SetIsPaused(false);
                //this.playerHud.SetActive(true);
                this.pauseMenu.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }
}
