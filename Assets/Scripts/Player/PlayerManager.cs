using UnityEngine;

public class PlayerManager : AbstractSingleton<PlayerManager>
{
    private GameObject playerOne;
    private GameObject playerTwo;
    private GameObject playerTwoHud;
    private int numberOfActivePlayers;
    private bool hasRetrievedChildren = false;
    private bool wasSceneLoaded;

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
        this.wasSceneLoaded = false;

        playerOne.SetActive(true);
        this.numberOfActivePlayers = 1;

        playerTwo.SetActive(false);
        playerTwoHud.SetActive(false);

        PositionPlayers();
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

    private void FixedUpdate()
    {
        if (this.wasSceneLoaded == true)
        {
            PositionPlayers();
            this.wasSceneLoaded = false;
        }
    }

    void OnLevelWasLoaded(int level)
    {
        PositionPlayers();
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

    public void PositionPlayers()
    {
        if (this.playerOne != null && this.playerTwo != null)
        {
            GameObject playerOneSpawn = GameObject.Find("Player1Spawn");
            GameObject playerTwoSpawn = GameObject.Find("Player2Spawn");

            this.playerOne.transform.position = playerOneSpawn.transform.position;
            this.playerOne.transform.rotation = playerOneSpawn.transform.rotation;

            this.playerTwo.transform.position = playerTwoSpawn.transform.position;
            this.playerTwo.transform.rotation = playerTwoSpawn.transform.rotation;
        }
    }
}
