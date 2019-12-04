using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : AbstractSingleton<PlayerManager>
{
    private GameObject playerOne;
    private GameObject playerTwo;
    private GameObject playerOneHud;
    private GameObject playerTwoHud;
    private GameObject playerTwoJoinPrompt;
    private GameObject playerOneDied;
    private GameObject playerTwoDied;

    private int numberOfActivePlayers;
    private bool hasRetrievedChildren = false;

    private ControllerManager controllerManager;

    protected override void Awake()
    {
        if (instance == null)
        {
            instance = this as PlayerManager;
            DontDestroyOnLoad(this.gameObject);
            if(!this.hasRetrievedChildren)
            {
                playerOne = transform.GetChild(0).gameObject;
                playerTwo = transform.GetChild(1).gameObject;
                playerOneHud = transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
                playerTwoHud = transform.GetChild(2).gameObject.transform.GetChild(1).gameObject;
                playerTwoJoinPrompt = transform.GetChild(2).gameObject.transform.GetChild(2).gameObject;
                playerOneDied = transform.GetChild(2).gameObject.transform.GetChild(3).gameObject;
                playerTwoDied = transform.GetChild(2).gameObject.transform.GetChild(4).gameObject;
                this.hasRetrievedChildren = true;
            }
        }
        else if (instance != this as PlayerManager)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        controllerManager = ControllerManager.GetInstance();

        playerOne.SetActive(true);
        this.numberOfActivePlayers = 1;

        playerTwo.SetActive(false);
        playerTwoHud.SetActive(false);
        playerTwoJoinPrompt.SetActive(false);
        playerOneDied.SetActive(false);
        playerTwoDied.SetActive(false);
    }

    void Update()
    {
        if (playerTwo.activeSelf == false && !(controllerManager.GetPlayerTwoController() is NullController) && !playerTwo.GetComponent<Player>().IsPermaDead())
        {
            playerTwoJoinPrompt.SetActive(true);
            if (controllerManager.GetPlayerTwoController().GetControllerActions().action1.IsPressed)
            {
                playerTwo.SetActive(true);
                playerTwoHud.SetActive(true);
                playerTwoJoinPrompt.SetActive(false);
                this.numberOfActivePlayers++;
            }
        }
        else if (playerTwo.activeSelf == false && (controllerManager.GetPlayerTwoController() is NullController))
        {
            playerTwoJoinPrompt.SetActive(false);
            playerTwoDied.SetActive(false);
        }
        else if (playerTwo.GetComponent<Player>().IsPermaDead())
        {
            playerTwoHud.SetActive(false);
            playerTwoDied.SetActive(true);
        }
        else if (!playerTwo.GetComponent<Player>().IsPermaDead())
        {
            playerTwoHud.SetActive(true);
            playerTwoDied.SetActive(false);
        }
        else if (playerOne.GetComponent<Player>().IsPermaDead())
        {
            playerOneHud.SetActive(false);
            playerOneDied.SetActive(true);
        }
        else if (!playerOne.GetComponent<Player>().IsPermaDead())
        {
            playerOneHud.SetActive(true);
            playerOneDied.SetActive(false);
        }
    }

    void LateUpdate()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.buildIndex == 0 || scene.buildIndex == 1 || scene.name == "HubMainMenu")
        {
            Destroy(this.gameObject);
        }
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

    public void RemovePlayer2()
    {
        if (!playerTwo.GetComponent<Player>().IsDead() && !playerOne.GetComponent<Player>().IsDead())
        {
            playerTwo.SetActive(false);
            playerTwoHud.SetActive(false);
            this.numberOfActivePlayers = 1;
        }
    }
}
