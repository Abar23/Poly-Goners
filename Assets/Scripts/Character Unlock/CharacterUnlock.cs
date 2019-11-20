using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUnlock : MonoBehaviour
{
    public int CharacterIndex;
    public int Price = 50;
    public GameObject CellDoor;

    private GameObject player1;
    private GameObject player2;
    private GameObject textPanel;
    private GameObject notEnoughGoldText;
    private GameObject activeChar;
    private float promptActivationDistance = 1f;
    private ParticleSystem ps;
    private int subCharIndex = 0;

    void Start()
    {
        player1 = PlayerManager.GetInstance().GetPlayerOneGameObject();
        player2 = PlayerManager.GetInstance().GetPlayerTwoGameObject();

        textPanel = GetComponentInChildren<Canvas>().transform.GetChild(0).gameObject;
        textPanel.gameObject.transform.GetChild(0).transform.gameObject.GetComponent<Text>().text = "Cost: " + Price + " Gold";
        notEnoughGoldText = textPanel.transform.GetChild(5).gameObject;
        notEnoughGoldText.SetActive(true);
        textPanel.SetActive(false);
        ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null)
            ps.gameObject.SetActive(false);

        for (int i = 1; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.activeSelf == true)
            {
                activeChar = gameObject.transform.GetChild(i).gameObject;
                subCharIndex = i - 1;
                break;
            }
        }
    }

    void Update()
    {
        if (activeChar.activeSelf)
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
                            player1.GetComponent<Player>().UpdateAnimator();
                            player1.GetComponent<Inventory>().DecreaseGold(Price);
                            //CellDoor.SetActive(false);
                            if (ps != null)
                                ps.gameObject.SetActive(true);
                            textPanel.SetActive(false);
                            activeChar.SetActive(false);
                            
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
                            player2.GetComponent<Player>().UpdateAnimator();
                            player2.GetComponent<Inventory>().DecreaseGold(Price);
                            //CellDoor.SetActive(false);
                            if (ps != null)
                                ps.gameObject.SetActive(true);
                            textPanel.SetActive(false);
                            activeChar.SetActive(false);
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
            textPanel.SetActive(false);
        }
    }
}
