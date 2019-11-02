using UnityEngine;

public class PauseManager : MonoBehaviour
{
    //public GameObject playerHud;

    public GameObject pauseMenu;

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
                Time.timeScale = 0.0f;
            }
            else
            {
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
