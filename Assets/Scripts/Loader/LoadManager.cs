using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadManager : MonoBehaviour
{

    [System.Serializable]
    public struct LoadingItem
    {
        public GameObject Object;

        [Range(0, 10)]
        public float FadeInTime;

        [Range(0, 10)]
        public float FadeOutTime;

        [Range(0, 10)]
        public float Duration;

    }

    public List<LoadingItem> PresentItems;

    public Image Mask;

    public string NextSceneName;

    private AsyncOperation op;

    void Start()
    {
        PresentItems[0].Object.SetActive(true);
        StartCoroutine(FadeIn(PresentItems[0]));
        StartCoroutine(LoadAsync(NextSceneName));
    }

    IEnumerator FadeIn(LoadingItem item)
    {
        float time = 0;
        while (time <= item.FadeInTime)
        {
            Color color = Mask.color;
            color.a = Mathf.Lerp(1, 0, time / item.FadeInTime);
            Mask.color = color;
            time += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Hold(item));
    }

    IEnumerator Hold(LoadingItem item)
    {
        yield return new WaitForSeconds(item.Duration);
        StartCoroutine(FadeOut(item));
    }

    IEnumerator FadeOut(LoadingItem item)
    {
        float time = 0;
        while (time <= item.FadeOutTime)
        {
            Color color = Mask.color;
            color.a = Mathf.Lerp(0, 1, time / item.FadeOutTime);
            Mask.color = color;
            time += Time.deltaTime;
            yield return null;
        }
        if (item.Equals(PresentItems[PresentItems.Count - 1]))
        {
            Exit();
        }
        else
        {
            item.Object.SetActive(false);
            item = PresentItems[PresentItems.IndexOf(item) + 1];
            item.Object.SetActive(true);
            StartCoroutine(FadeIn(item));
        }
    }

    IEnumerator LoadAsync(string sceneName)
    {
        op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        while (!op.isDone)
        {
            yield return null;
        }
        
    }

    void Exit()
    {
        op.allowSceneActivation = true;
    }

}
