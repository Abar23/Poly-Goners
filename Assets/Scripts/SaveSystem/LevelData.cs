using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class LevelData
{
    public int levelIndex { get; private set; }
    public float[] playerPosition { get; private set; }
    public int numberOfCollectedItemsByPlayer { get; private set; }
    public bool hasPlayerWon { get; private set; }
    public bool[] activeCollectibles { get; private set; }
    
    public LevelData(PlayerController playerController, GameObject collectibles)
    {
        this.levelIndex = SceneManager.GetActiveScene().buildIndex;

        this.playerPosition = new float[3];
        this.playerPosition[0] = playerController.transform.position.x;
        this.playerPosition[1] = playerController.transform.position.y;
        this.playerPosition[2] = playerController.transform.position.z;

        this.numberOfCollectedItemsByPlayer = playerController.numberOfCollectedItems;

        this.hasPlayerWon = playerController.hasPlayerWon;

        this.activeCollectibles = new bool[collectibles.transform.childCount];
        for(int i = 0; i < this.activeCollectibles.Length; i++)
        {
            GameObject childGameObject = collectibles.transform.GetChild(i).gameObject;
            this.activeCollectibles[i] = childGameObject.activeSelf;
        }
    }
}
