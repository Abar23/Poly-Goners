using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public enum Level 
    { 
        HubWorld, 
        DungeonLevel1,
        SnowLevel, 
        SpaceLevel, 
        CastleDungeonLevel,
        CastleHub,
        JailRoom,
        CastleArmoury
    };
    public Level level;
    private const float k_ExitTime = 2f;

    void OnTriggerEnter(Collider other)
    {
        if (level != null) {
            if (other.gameObject.tag == "Player") {
                LoadLevel(level);
            }
        }
    }

    public void LoadLevel(LevelLoader.Level level)
    {
        switch (level)
        {
            case LevelLoader.Level.HubWorld:
                LoadHubWorld();
                break;
            case LevelLoader.Level.DungeonLevel1:
                LoadDungeon1();
                break;
            case LevelLoader.Level.SnowLevel:
                LoadSnowLevel();
                break;
            case LevelLoader.Level.SpaceLevel:
                LoadSpaceLevel();
                break;
            case LevelLoader.Level.CastleDungeonLevel:
                LoadCastleDungeonLevel();
                break;
            case LevelLoader.Level.CastleHub:
                LoadCastleHub();
                break;
            case LevelLoader.Level.JailRoom:
                LoadJailRoom();
                break;
            case LevelLoader.Level.CastleArmoury:
                LoadCastleArmoury();
                break;
            default:
                Debug.Log("ERROR: Level not found in LevelFactory.");
                break;
        }
    }

    private void LoadHubWorld()
    {
        StartCoroutine(LoadScene("OutdoorHubWorld"));
    }

    private void LoadDungeon1()
    {
        StartCoroutine(LoadScene("DungeonLevel1"));
    }

    private void LoadSnowLevel()
    {
        StartCoroutine(LoadScene("SnowLevel"));
    }

    private void LoadSpaceLevel()
    {
        StartCoroutine(LoadScene("SpaceLevel"));
    }

    private void LoadCastleDungeonLevel()
    {
        StartCoroutine(LoadScene("CastleDungeonLevel"));
    }

    private void LoadCastleHub()
    {
        StartCoroutine(LoadScene("CastleHub"));
    }

    private void LoadJailRoom()
    {
        StartCoroutine(LoadScene("Jail"));
    }

    private void LoadCastleArmoury()
    {
        StartCoroutine(LoadScene("CastleArmoury"));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        Animator animator = GameObject.Find("SceneTransition").GetComponentInChildren<Animator>();
        Collider collider = this.GetComponent<Collider>();
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        collider.enabled = false;
        animator.SetTrigger("Exit");
        yield return new WaitForSeconds(k_ExitTime);
        op.allowSceneActivation = true;
    }
}
