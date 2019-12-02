using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Text ButtonText;
    public AudioClip ClickClip;
    public AudioClip HighlightClip;
    public Slider MusicSlider;
    public Slider FxSlider;
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

    public void FixedUpdate()
    {
        if (PlayerPrefs.HasKey("fx"))
        {
            clickSource.volume = PlayerPrefs.GetFloat("fx");
            highlightSource.volume = PlayerPrefs.GetFloat("fx");
        }
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
        StartCoroutine(DelayNextMenuLoad());
    }

    IEnumerator DelayNextMenuLoad()
    {
        clickSource.PlayOneShot(clickSource.clip);
        yield return new WaitForSecondsRealtime(clickSource.clip.length);
        //if (EventSystem.current != null)
        //    EventSystem.current.SetSelectedGameObject(null);
        transform.localScale = new Vector3(1f, 1f, 1f);
        ResetButtonColors();
        CurrentPanel.SetActive(false);
        CurrentTitle.SetActive(false);
        NextPanel.SetActive(true);
        NextTitle.SetActive(true);
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
        if (highlightSource != null)
            highlightSource.PlayOneShot(highlightSource.clip);
        ButtonText.color = _textHoverColor;
        GetComponent<Image>().color = _buttonHoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonText.color = _textDefaultColor;
        GetComponent<Image>().color = _buttonDefaultColor;
    }


    // For keyboard / controller highlighting of buttons
    public void OnSelect(BaseEventData eventData)
    {
        if (highlightSource != null)
            highlightSource.PlayOneShot(highlightSource.clip);
        ButtonText.color = _textHoverColor;
        Image bgnd = GetComponent<Image>();
        if (bgnd != null)
            bgnd.color = _buttonHoverColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {        
        ButtonText.color = _textDefaultColor;
        Image bgnd = GetComponent<Image>();
        if (bgnd != null)
            bgnd.color = _buttonDefaultColor;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1.0f;
    }

    public void ResetButtonColors()
    {
        ButtonText.color = _textDefaultColor;
        Image bgnd = GetComponent<Image>();
        if (bgnd != null)
            bgnd.color = _buttonDefaultColor;
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

    public void ApplyVolume()
    {
        clickSource.PlayOneShot(clickSource.clip);
        PlayerPrefs.SetFloat("music", MusicSlider.value);
        PlayerPrefs.SetFloat("fx", FxSlider.value);
    }

    public void ResetPlayerPrefs()
    {
        float music = 0f;
        float fx = 0f;

        if (PlayerPrefs.HasKey("music"))
            music = PlayerPrefs.GetFloat("music");
        if (PlayerPrefs.HasKey("fx"))
            fx = PlayerPrefs.GetFloat("fx");

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetFloat("music", music);
        PlayerPrefs.SetFloat("fx", fx);
    }
}
