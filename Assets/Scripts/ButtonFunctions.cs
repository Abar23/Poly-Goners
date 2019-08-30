using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Text ButtonText;

    private readonly Color _textHoverColor = new Color(0.24f, 0.14f, 0.22f, 1);
    private readonly Color _textDefaultColor = new Color(1, 1, 1, 1);
    private readonly Color _buttonHoverColor = new Color(1, 1, 1, 1);
    private readonly Color _buttonDefaultColor = new Color(1, 1, 1, 0);

    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
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
        ButtonText.color = _textHoverColor;
        GetComponent<Image>().color = _buttonHoverColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ButtonText.color = _textDefaultColor;
        GetComponent<Image>().color = _buttonDefaultColor;
    }
}
