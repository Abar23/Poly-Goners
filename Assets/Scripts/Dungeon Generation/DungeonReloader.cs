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
            StartCoroutine(ReloadScene());
        }
    }

    private IEnumerator ReloadScene()
    {
        Collider collider = this.GetComponent<Collider>();
        Animator animator = GameObject.Find("SceneTransition").GetComponentInChildren<Animator>();
        Scene scene = SceneManager.GetActiveScene();
        AsyncOperation op = SceneManager.LoadSceneAsync(scene.name);
        op.allowSceneActivation = false;
        collider.enabled = false;
        animator.SetTrigger("Exit");
        yield return new WaitForSeconds(k_ExitTime);
        OnReload.Invoke();
        op.allowSceneActivation = true;
    }
}
