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

    public Text ProgressStatus;

    public Slider ProgressBar;

    public int NextSceneIndex;

    private AsyncOperation op;

    void Start()
    {
        PresentItems[0].Object.SetActive(true);
        StartCoroutine(FadeIn(PresentItems[0]));
        StartCoroutine(LoadAsync(NextSceneIndex));
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

    IEnumerator LoadAsync(int sceneIndex)
    {
        op = SceneManager.LoadSceneAsync(sceneIndex);
        op.allowSceneActivation = false;
        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            ProgressStatus.text = String.Format("{0,0:F0} %", progress * 100f);
            ProgressBar.value = progress;
            if (Mathf.Abs(progress - 1f) < 0.001f)
            {
                ProgressStatus.text = "Done";
            }
            yield return null;
        }
        
    }

    void Exit()
    {
        //#if UNITY_EDITOR
        //// Application.Quit() does not work in the editor so
        //// UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        //UnityEditor.EditorApplication.isPlaying = false;
        //#else
        //Application.Quit();
        //#endif
        op.allowSceneActivation = true;
    }

}
