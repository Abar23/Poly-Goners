using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelFactory
{
    public static void LoadLevel(LevelLoader.Level level) {
        switch(level) {
            case LevelLoader.Level.HubWorld:
                LoadHubWorld();
                break;
            case LevelLoader.Level.DungeonLevel1:
                LoadDungeon1();
                break;
            default:
                Debug.Log("ERROR: Level not found in LevelFactory.");
                break;
        }
    }

    private static void LoadHubWorld() {
        SceneManager.LoadScene("OutdoorHubWorld");
    }

    private static void LoadDungeon1() {
        SceneManager.LoadScene("DungeonLevel1");
    }
}