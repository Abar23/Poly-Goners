using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private GameObject playerOne;
    private GameObject playerTwo;
    private GameObject playerTwoHud;

    private ControllerManager controllerManager;

    void Start()
    {
        controllerManager = ControllerManager.GetInstance().GetComponent<ControllerManager>();

        playerOne = transform.GetChild(0).gameObject;
        playerTwo = transform.GetChild(1).gameObject;
        playerTwoHud = transform.GetChild(2).gameObject.transform.GetChild(1).gameObject;

        playerOne.SetActive(true);
        playerTwo.SetActive(false);
        playerTwoHud.SetActive(false);
    }

    void Update()
    {
        if (playerTwo.activeSelf == false && !(controllerManager.GetPlayerTwoController() is NullController))
        {
            playerTwo.SetActive(true);
            playerTwoHud.SetActive(true);
        }
    }
}
