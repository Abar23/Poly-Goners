using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidBody;
    private int totalNumberOfCollectables;

    public int numberOfCollectedItems { get; private set; }
    public bool hasPlayerWon { get; private set; }
    public Text collectedText;
    public Text winText;
    public float speed;

    void Start()
    {
        this.rigidBody = GetComponent<Rigidbody>();
        this.totalNumberOfCollectables = GameObject.Find("Collectibles").transform.childCount;
        this.hasPlayerWon = false;
        this.numberOfCollectedItems = 0;

        if (SaveSystem.shouldLevelBeLoaded)
        {
            LevelData levelData = SaveSystem.loadedLevelData;
            this.hasPlayerWon = levelData.hasPlayerWon;
            this.numberOfCollectedItems = levelData.numberOfCollectedItemsByPlayer;
            float[] position = levelData.playerPosition;
            this.transform.position = new Vector3(position[0], position[1], position[2]);
        }
        
        this.winText.text = "";
        this.UpdateCollectedText();
    }

    void FixedUpdate()
    {
        if(!PauseManager.isGamePaused)
        {
            this.rigidBody.AddForce(Input.GetAxis("Horizontal") * this.speed, 0.0f, Input.GetAxis("Vertical") * this.speed);
        }

        if (this.hasPlayerWon == true)
        {
            this.winText.text = "You Win!!";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Collectable")
        {
            this.numberOfCollectedItems++;

            this.UpdateCollectedText();

            if (this.numberOfCollectedItems == this.totalNumberOfCollectables)
            {
                this.hasPlayerWon = true;
            }
        }
    }

    private void UpdateCollectedText()
    {
        this.collectedText.text = this.numberOfCollectedItems.ToString() + " / " + this.totalNumberOfCollectables.ToString() + " Flowers";
    }
}
