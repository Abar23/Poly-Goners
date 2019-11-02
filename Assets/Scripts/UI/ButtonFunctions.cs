using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonFunctions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Text ButtonText;
    public AudioClip ClickClip;
    public AudioClip HighlightClip;
    public GameObject CurrentPanel;
    public GameObject CurrentTitle;
    public GameObject NextPanel;
    public GameObject NextTitle;

    private AudioSource clickSource;
    private AudioSource highlightSource;
    private readonly Color _textHoverColor = new Color(0.24f, 0.14f, 0.22f, 1);
    private readonly Color _textDefaultColor = new Color(1, 1, 1, 1);
    private readonly Color _buttonHoverColor = new Color(1, 1, 1, 1);
    private readonly Color _buttonDefaultColor = new Color(1, 1, 1, 0);

    public void Start()
    {
        clickSource = gameObject.AddComponent<AudioSource>();
        highlightSource = gameObject.AddComponent<AudioSource>();

        clickSource.clip = ClickClip;
        highlightSource.clip = HighlightClip;

        clickSource.playOnAwake = false;
        highlightSource.playOnAwake = false;
        highlightSource.volume = 0.25f;
    }


    public void LoadSceneByIndex(int sceneIndex)
    {
        StartCoroutine(DelaySceneLoad(sceneIndex));
    }

    IEnumerator DelaySceneLoad(int sceneIndex)
    {
        clickSource.PlayOneShot(clickSource.clip);
        yield return new WaitForSecondsRealtime(clickSource.clip.length);
        SceneManager.LoadScene(sceneIndex);

        if (SceneManager.GetActiveScene().buildIndex > 1 && sceneIndex == 1)
        {
            Destroy(PlayerManager.GetInstance().gameObject);
        }
    }

    public void LoadNextMenu()
    {
        StartCoroutine(DelayMenuLoad());
    }

    IEnumerator DelayMenuLoad()
    {
        clickSource.PlayOneShot(clickSource.clip);
        yield return new WaitForSecondsRealtime(clickSource.clip.length);
        CurrentPanel.SetActive(false);
        CurrentTitle.SetActive(false);
        NextPanel.SetActive(true);
        NextTitle.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        transform.localScale = new Vector3(1f, 1f, 1f);
        OnDeselect(null);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // For mouse highlighting of buttons
    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightSource.PlayOneShot(highlightSource.clip);
        ButtonText.color = _textHoverColor;
        GetComponent<Image>().color = _buttonHoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonText.color = _textDefaultColor;
        GetComponent<Image>().color = _buttonDefaultColor;
    }


    // For keyboard / controller highlighing of buttons
    public void OnSelect(BaseEventData eventData)
    {
        highlightSource.PlayOneShot(highlightSource.clip);
        ButtonText.color = _textHoverColor;
        GetComponent<Image>().color = _buttonHoverColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {        
        ButtonText.color = _textDefaultColor;
        GetComponent<Image>().color = _buttonDefaultColor;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1.0f;
    }

    public void SaveLevel(DataEncapsulator dataEncapsulator)
    {
        clickSource.PlayOneShot(clickSource.clip);
        SaveSystem.SaveLevel(dataEncapsulator.playerController, dataEncapsulator.collectibles);
        ShouldLevelLoad(false);
    }

    public void LoadLevel()
    {
        SaveSystem.LoadLevel();
        ShouldLevelLoad(true);
        LoadSceneByIndex(SaveSystem.loadedLevelData.levelIndex);
    }

    public void ShouldLevelLoad(bool status)
    {
        SaveSystem.shouldLevelBeLoaded = status;
    }

    public void RemovePlayer2()
    {
        PlayerManager.GetInstance().RemovePlayer2();
    }
}
