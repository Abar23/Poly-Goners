using UnityEngine;

public class PlayerManager : AbstractSingleton<PlayerManager>
{
    private GameObject playerOne;
    private GameObject playerTwo;
    private GameObject playerTwoHud;
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
