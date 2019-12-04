using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubSpawnController : MonoBehaviour
{
    public GameObject campSpawn;
    public GameObject snowLevelSpawn;
    public GameObject castleLevelSpawn;
    public GameObject armorySpawn;
    public GameObject jailSpawn;

    private string previousScene;

    private static string playerPrefsLevelString = "PreviousScene";

    void Start()
    {
        this.previousScene = "";
        if(PlayerPrefs.HasKey("PreviousScene"))
        {
            this.previousScene = PlayerPrefs.GetString("PreviousScene");
        }

        if(this.previousScene == "" || this.previousScene == "HubMainMenu")
        {
            this.campSpawn.SetActive(true);
        }
        if (this.previousScene == "SnowLevel")
        {
            this.snowLevelSpawn.SetActive(true);
        }
        if (this.previousScene == "CastleDungeonLevel")
        {
            this.castleLevelSpawn.SetActive(true);
        }
        if (this.previousScene == "Jail")
        {
            this.jailSpawn.SetActive(true);
        }
        if (this.previousScene == "CastleArmoury")
        {
            this.armorySpawn.SetActive(true);
        }

        PlayerPrefs.DeleteKey(playerPrefsLevelString);
    }

    public static string getPlayerPrefsKey()
    {
        return playerPrefsLevelString;
    }
}
