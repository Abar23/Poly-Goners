﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public enum Level { HubWorld, DungeonLevel1, SnowLevel };
    public Level level;
    [SerializeField] private Animator m_Animator;
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

    private IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        m_Animator.SetTrigger("Exit");
        yield return new WaitForSeconds(k_ExitTime);
        op.allowSceneActivation = true;
    }
}
