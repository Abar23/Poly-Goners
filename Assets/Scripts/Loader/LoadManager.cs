using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    public List<LoadingItem> LoadingItems;

    public Image Mask;

    // Start is called before the first frame update
    void Start()
    {
        LoadingItems[0].Object.SetActive(true);
        StartCoroutine(FadeIn(LoadingItems[0]));
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    IEnumerator FadeIn(LoadingItem item)
    {
        float updateTime = 0.05f;
        float time = 0;
        while (time <= item.FadeInTime)
        {
            Color color = Mask.color;
            color.a = Mathf.Lerp(1, 0, time / item.FadeInTime);
            Mask.color = color;
            time += updateTime;
            yield return new WaitForSeconds(updateTime);
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
        float updateTime = 0.05f;
        float time = 0;
        while (time <= item.FadeOutTime)
        {
            Color color = Mask.color;
            color.a = Mathf.Lerp(0, 1, time / item.FadeOutTime);
            Mask.color = color;
            time += updateTime;
            yield return new WaitForSeconds(updateTime);
        }
        if (item.Equals(LoadingItems[LoadingItems.Count - 1]))
        {
            Exit();
        }
        else
        {
            item.Object.SetActive(false);
            item = LoadingItems[LoadingItems.IndexOf(item) + 1];
            item.Object.SetActive(true);
            StartCoroutine(FadeIn(item));
        }
    }

    void Exit()
    {
        #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

}
