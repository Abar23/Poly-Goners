using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidBody;
    private int numberOfCollectedItems;
    private bool hasPlayerWon;
    private int totalNumberOfCollectables;

    public Text collectedText;
    public Text winText;
    public float speed;

    void Start()
    {
        this.rigidBody = GetComponent<Rigidbody>();
        this.numberOfCollectedItems = 0;
        this.hasPlayerWon = false;
        this.totalNumberOfCollectables = GameObject.Find("Collectables").transform.childCount;
        this.winText.text = "";
        this.UpdateCollectedText();
    }

    void Update()
    {
        this.rigidBody.AddForce(Input.GetAxis("Horizontal") * this.speed, 0.0f, Input.GetAxis("Vertical") * this.speed);

        if(this.hasPlayerWon == true)
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
