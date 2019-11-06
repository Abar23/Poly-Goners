using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DungeonReloader : MonoBehaviour
{
    public UnityEvent OnReload;
    private const float k_ExitTime = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            DungeonCompletionTracker.GetInstance().IncreaseNumberOfCompletedDungeons();
            OnReload.Invoke();
            StartCoroutine(ReloadScene());
        }
    }

    private IEnumerator ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        AsyncOperation op = SceneManager.LoadSceneAsync(scene.name);
        op.allowSceneActivation = false;
        yield return new WaitForSeconds(k_ExitTime);
        op.allowSceneActivation = true;
    }
}
