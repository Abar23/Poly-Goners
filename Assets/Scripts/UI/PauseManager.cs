using UnityEngine;

public class PauseManager : MonoBehaviour
{
    //public GameObject playerHud;

    public GameObject pauseMenu;

    public static bool isGamePaused;

    private ControllerManager cm;

    void Start()
    {
        isGamePaused = false;
        //this.playerHud.SetActive(true);
        this.pauseMenu.SetActive(false);
        cm = ControllerManager.GetInstance().GetComponent<ControllerManager>();
    }

    void LateUpdate()
    {
        if(cm.GetPlayerOneController().GetControllerActions().start.WasPressed)
        {
            if(!isGamePaused)
            {
                isGamePaused = true;
                //this.playerHud.SetActive(false);
                this.pauseMenu.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else
            {
                isGamePaused = false;
                //this.playerHud.SetActive(true);
                this.pauseMenu.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }
}
