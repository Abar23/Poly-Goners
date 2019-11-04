using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DungeonReloader : MonoBehaviour
{
    public UnityEvent OnReload;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            DungeonCompletionTracker.GetInstance().IncreaseNumberOfCompletedDungeons();
            OnReload.Invoke();
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
