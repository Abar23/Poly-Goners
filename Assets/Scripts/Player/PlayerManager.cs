using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : AbstractSingleton<PlayerManager>
{
    private GameObject playerOne;
    private GameObject playerTwo;
    private GameObject playerTwoHud;
    private int numberOfActivePlayers;

    private ControllerManager controllerManager;

    void Start()
    {
        controllerManager = ControllerManager.GetInstance();

        playerOne = transform.GetChild(0).gameObject;
        playerTwo = transform.GetChild(1).gameObject;
        playerTwoHud = transform.GetChild(2).gameObject.transform.GetChild(1).gameObject;

        playerOne.SetActive(true);
        this.numberOfActivePlayers = 1;

        playerTwo.SetActive(false);
        playerTwoHud.SetActive(false);

        PositionPlayers(SceneManager.GetActiveScene(), 0);
    }

    void Update()
    {
        if (playerTwo.activeSelf == false && !(controllerManager.GetPlayerTwoController() is NullController))
        {
            playerTwo.SetActive(true);
            playerTwoHud.SetActive(true);
            this.numberOfActivePlayers++;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += PositionPlayers;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= PositionPlayers;
    }

    public GameObject GetPlayerOneGameObject()
    {
        return this.playerOne;
    }

    public GameObject GetPlayerTwoGameObject()
    {
        return this.playerTwo;
    }

    public int GetNumberOfActivePlayers()
    {
        return this.numberOfActivePlayers;
    }

    public void PositionPlayers(Scene scene, LoadSceneMode mode)
    {
        if (this.playerOne != null)
        {
            GameObject playerOneSpawn = GameObject.Find("Player1Spawn");

            this.playerOne.transform.position = playerOneSpawn.transform.position;
            this.playerOne.transform.rotation = playerOneSpawn.transform.rotation;

        }

        if (this.playerTwo != null)
        {
            GameObject playerTwoSpawn = GameObject.Find("Player2Spawn");

            this.playerTwo.transform.position = playerTwoSpawn.transform.position;
            this.playerTwo.transform.rotation = playerTwoSpawn.transform.rotation;
        }
    }
}
