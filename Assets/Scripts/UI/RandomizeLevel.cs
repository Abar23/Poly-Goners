using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class RandomizeLevel : MonoBehaviour
{
    int tileSet;

    void Start() {
        // randomize level type (0 = maze, 1 = swamp)
        tileSet = Random.Range(0, 2);
        
        if (tileSet == 0) {
            SceneManager.LoadScene("RandomMazeLevel");
        } else {
            SceneManager.LoadScene("RandomSwampLevel");
        }
    }
}
