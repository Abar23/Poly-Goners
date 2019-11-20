using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileOccluder : MonoBehaviour
{
    public float tileDimentions;
    private float occlusionDistance;
    DungeonDoorways dungeonDoorways;
    private bool isRendered;

    void Start()
    {
        this.occlusionDistance = Mathf.Sqrt(Mathf.Pow(this.tileDimentions / 1.85f, 2.0f) * 2.0f);
        this.dungeonDoorways = this.gameObject.GetComponent<DungeonDoorways>();
        this.isRendered = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerManager playerManager = PlayerManager.GetInstance();
        int numberOfActivePlayers = playerManager.GetNumberOfActivePlayers();
        GameObject playerOne = playerManager.GetPlayerOneGameObject();
        GameObject playerTwo = playerManager.GetPlayerTwoGameObject();

        if (numberOfActivePlayers == 1 && playerOne != null)
        {
            CheckOcclusionWithOnePlayer(playerOne);
        }
        else if (numberOfActivePlayers == 1 && playerTwo != null)
        {
            CheckOcclusionWithOnePlayer(playerTwo);
        }
        else if (numberOfActivePlayers == 2)
        {
            CheckOcclusionWithBothPlayers(playerOne, playerTwo);
        }
    }

    private void CheckOcclusionWithOnePlayer(GameObject player)
    {
        bool shouldBeRendered = false;
        float distance = Vector3.Distance(this.transform.position, player.transform.position);

        if (distance <= this.occlusionDistance)
        {
            shouldBeRendered = true;
        }
        else
        {
            foreach (Transform transform in this.dungeonDoorways.DungeonDoorwayPositions)
            {
                distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance <= this.occlusionDistance)
                {
                    shouldBeRendered = true;
                    break;
                }
            }
        }

        if (!shouldBeRendered)
        {
            if (this.isRendered)
            {
                DerenderTile();
            }
        }
        else
        {
            if (!this.isRendered)
            {
                RenderTile();
            }
        }
    }

    private void CheckOcclusionWithBothPlayers(GameObject playerOne, GameObject playerTwo)
    {
        bool shouldBeRendered = false;
        float playerOneDistance = Vector3.Distance(this.gameObject.transform.position, playerOne.transform.position);
        float playerTwoDistance = Vector3.Distance(this.gameObject.transform.position, playerTwo.transform.position);

        if (playerOneDistance <= this.occlusionDistance || playerTwoDistance <= this.occlusionDistance)
        {
            shouldBeRendered = true;
        }
        else
        {
            foreach (Transform transform in this.dungeonDoorways.DungeonDoorwayPositions)
            {
                playerOneDistance = Vector3.Distance(transform.position, playerOne.transform.position);
                playerTwoDistance = Vector3.Distance(transform.position, playerTwo.transform.position);
                if (playerOneDistance <= this.occlusionDistance || playerTwoDistance <= this.occlusionDistance)
                {
                    shouldBeRendered = true;
                    break;
                }
            }
        }

        if (!shouldBeRendered)
        {
            if (this.isRendered)
            {
                DerenderTile();
            }
        }
        else
        {
            if (!this.isRendered)
            {
                RenderTile();
            }
        }
    }

    private void DerenderTile()
    {
        this.isRendered = false;
        SetActiveStateOfChildren(this.isRendered);
    }

    private void RenderTile()
    {
        this.isRendered = true;
        SetActiveStateOfChildren(this.isRendered);
    }

    private void SetActiveStateOfChildren(bool activeState)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(activeState);
        }
    }
}
