using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUnlock : MonoBehaviour
{
    public int CharacterIndex;
    public int Price = 50;
    public GameObject CellDoor;
    public List<AnimationClip> Animations;
    public List<AnimationClip> Dances;

    private GameObject player1;
    private GameObject player2;
    private GameObject textPanel;
    private GameObject notEnoughGoldText;
    private GameObject activeChar;
    private float promptActivationDistance = 1f;
    private ParticleSystem characterPS;
    private ParticleSystem doorPS;
    private int subCharIndex = 0;
    private bool isUnlocked = false;

    private Animator anim;
    private AnimatorOverrideController animatorOverrideController;
    private readonly System.Random rnd = new System.Random();
    private int randDance;

    void Start()
    {
        anim = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = animatorOverrideController;
        int r = rnd.Next(Animations.Count);
        animatorOverrideController["PRIMARY_ACTION"] = Animations[r];

        player1 = PlayerManager.GetInstance().GetPlayerOneGameObject();
        player2 = PlayerManager.GetInstance().GetPlayerTwoGameObject();

        textPanel = GetComponentInChildren<Canvas>().transform.GetChild(0).gameObject;
        textPanel.gameObject.transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = "Cost: " + Price + " Gold";
        notEnoughGoldText = textPanel.transform.GetChild(5).gameObject;
        notEnoughGoldText.SetActive(true);
        textPanel.SetActive(false);
        characterPS = GetComponentInChildren<ParticleSystem>();
        if (characterPS != null)
            characterPS.gameObject.SetActive(false);

        doorPS = CellDoor.GetComponentInChildren<ParticleSystem>();
        if (doorPS != null)
            doorPS.gameObject.SetActive(false);
            
        for (int i = 1; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.activeSelf == true)
            {
                activeChar = gameObject.transform.GetChild(i).gameObject;
                subCharIndex = i - 1;
                break;
            }
        }

        if (PlayerPrefs.HasKey(activeChar.name))
        {
            isUnlocked = true;
            gameObject.transform.position = CellDoor.transform.position + gameObject.transform.forward * 0.5f + transform.right * -0.75f; 
        }
        else
            isUnlocked = false;


        randDance = rnd.Next(Dances.Count);
    }

    void Update()
    {
        if (!isUnlocked)
        {
            float distanceFromPlayer1 = Vector3.Distance(CellDoor.transform.position, player1.transform.position);
            float distanceFromPlayer2 = Vector3.Distance(CellDoor.transform.position, player2.transform.position);

            if (distanceFromPlayer1 <= promptActivationDistance || distanceFromPlayer2 <= promptActivationDistance)
            {
                // Display menu to player 1
                if (distanceFromPlayer1 <= promptActivationDistance)
                {
                    if (player1.GetComponent<Player>().CheckUseButtonPress())
                    {
                        if (player1.GetComponent<Inventory>().GetGold() >= Price)
                        {
                            player1.GetComponent<CharacterBox>().SetCharacter(CharacterIndex, subCharIndex);
                            player1.GetComponent<CharacterBox>().SetIcon(activeChar);
                            player1.GetComponent<Player>().UpdateAnimator();
                            player1.GetComponent<Inventory>().DecreaseGold(Price);
                            if (doorPS != null)
                                doorPS.gameObject.SetActive(true);
                            CellDoor.GetComponent<MeshRenderer>().enabled = false;
                            if (characterPS != null)
                                characterPS.gameObject.SetActive(true);
                            textPanel.SetActive(false);
                            gameObject.transform.position = CellDoor.transform.position + gameObject.transform.forward * 0.5f + transform.right * -0.75f;
                            isUnlocked = true;
                        }
                        else
                        {
                            notEnoughGoldText.SetActive(true);
                        }
                    }

                    textPanel.SetActive(true);
                    textPanel.transform.position = new Vector3(transform.position.x, player1.transform.position.y + 3.5f, transform.position.z);
                }

                // Display menu to player 2
                else if (distanceFromPlayer2 <= promptActivationDistance)
                {
                    if (player2.GetComponent<Player>().CheckUseButtonPress())
                    {
                        if (player2.GetComponent<Inventory>().GetGold() >= Price)
                        {
                            player2.GetComponent<CharacterBox>().SetCharacter(CharacterIndex, subCharIndex);
                            player2.GetComponent<CharacterBox>().SetIcon(activeChar);
                            player2.GetComponent<Player>().UpdateAnimator();
                            player2.GetComponent<Inventory>().DecreaseGold(Price);
                            if (doorPS != null)
                                doorPS.gameObject.SetActive(true);
                            CellDoor.GetComponent<MeshRenderer>().enabled = false;
                            if (characterPS != null)
                                characterPS.gameObject.SetActive(true);
                            textPanel.SetActive(false);
                            gameObject.transform.position = CellDoor.transform.position + gameObject.transform.forward * 0.5f + transform.right * -0.75f;
                            isUnlocked = true;
                        }
                        else
                        {
                            notEnoughGoldText.SetActive(true);
                        }
                    }

                    textPanel.SetActive(true);
                    textPanel.transform.position = new Vector3(transform.position.x, player2.transform.position.y + 3.5f, transform.position.z);
                }
                else
                {
                    notEnoughGoldText.SetActive(false);
                    textPanel.SetActive(false);
                }
            }

            else
            {
                notEnoughGoldText.SetActive(false);
                textPanel.SetActive(false);
            }
        }
        else
        {
            if (!PlayerPrefs.HasKey(activeChar.name))
                PlayerPrefs.SetInt(activeChar.name, 1);

            animatorOverrideController["PRIMARY_ACTION"] = Dances[randDance];
            textPanel.SetActive(false);
            CellDoor.GetComponent<MeshRenderer>().enabled = false;
            textPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
            textPanel.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Play as this character?";


            float distanceFromPlayer1 = Vector3.Distance(gameObject.transform.position, player1.transform.position);
            float distanceFromPlayer2 = Vector3.Distance(gameObject.transform.position, player2.transform.position);

            if (distanceFromPlayer1 <= promptActivationDistance || distanceFromPlayer2 <= promptActivationDistance)
            {
                // Display menu to player 1
                if (distanceFromPlayer1 <= promptActivationDistance)
                {
                    if (player1.GetComponent<Player>().CheckUseButtonPress())
                    {
                        player1.GetComponent<CharacterBox>().SetCharacter(CharacterIndex, subCharIndex);
                        player1.GetComponent<CharacterBox>().SetIcon(activeChar);
                        player1.GetComponent<Player>().UpdateAnimator();
                    }

                    textPanel.SetActive(true);
                    textPanel.transform.position = new Vector3(transform.position.x, player1.transform.position.y + 3.5f, transform.position.z);
                }

                // Display menu to player 2
                else if (distanceFromPlayer2 <= promptActivationDistance)
                {
                    if (player2.GetComponent<Player>().CheckUseButtonPress())
                    {
                        player2.GetComponent<CharacterBox>().SetCharacter(CharacterIndex, subCharIndex);
                        player2.GetComponent<CharacterBox>().SetIcon(activeChar);
                        player2.GetComponent<Player>().UpdateAnimator();
                    }

                    textPanel.SetActive(true);
                    textPanel.transform.position = new Vector3(transform.position.x, player2.transform.position.y + 3.5f, transform.position.z);
                }
                else
                {
                    notEnoughGoldText.SetActive(false);
                    textPanel.SetActive(false);
                }
            }

            else
            {
                notEnoughGoldText.SetActive(false);
                textPanel.SetActive(false);
            }
        }
    }
}
