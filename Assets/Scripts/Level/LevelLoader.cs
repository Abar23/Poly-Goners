using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public enum Level { HubWorld, DungeonLevel1 };
    public Level level;

    void OnTriggerEnter(Collider other)
    {
        if (level != null) {
            if (other.gameObject.tag == "Player") {
                LevelFactory.LoadLevel(level);
            }       
        }
    }
}
