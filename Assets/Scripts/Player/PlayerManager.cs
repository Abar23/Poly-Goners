using UnityEngine;

public class PlayerManager : AbstractSingleton<PlayerManager>
{
    private GameObject playerOne;
    private GameObject playerTwo;
    private GameObject playerTwoHud;
    private GameObject playerTwoJoinPrompt;
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
                playerTwoHud = transform.GetChild(2).gameObject.transform.GetChild(1).gameObject;
                playerTwoJoinPrompt = transform.GetChild(2).gameObject.transform.GetChild(2).gameObject;
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
}
