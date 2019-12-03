using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour
{
    public EventSystem EventSystem;
    public GameObject SelectedGameObject;
    public GameObject CurrentMenu;
    public GameObject CurrentTitle;
    public GameObject PreviousMenu;
    public GameObject PreviousTitle;
    public GameObject BackPrompt;
    public bool CanGoBack;

    private bool _buttonSelected;
    private GameObject firstSelected;
    private ControllerManager cm;

    private void Start()
    {
        if (BackPrompt != null)
            BackPrompt.SetActive(false);

        cm = ControllerManager.GetInstance();
        firstSelected = SelectedGameObject;
    }

    // Update is called once per frame
    void Update ()
	{
        if (!cm.GetPlayerOneController().isUsingKeyboard())
        {
            if (GetComponent<StandaloneInputModule>() != null)
                GetComponent<StandaloneInputModule>().enabled = false;
        }
        else
        {
            if (GetComponent<StandaloneInputModule>() != null)
                GetComponent<StandaloneInputModule>().enabled = true;
        }


        if (PreviousMenu != null && (cm.GetPlayerOneController().GetControllerActions().action2.WasPressed || cm.GetPlayerTwoController().GetControllerActions().action2.WasPressed))
        {
            Disable();
            CurrentMenu.SetActive(false);
            CurrentTitle.SetActive(false);
            PreviousMenu.SetActive(true);
            PreviousTitle.SetActive(true);
        }

        if (CanGoBack && BackPrompt.activeSelf == false)
            BackPrompt.SetActive(true);
        else if (!CanGoBack)
            BackPrompt.SetActive(false);
	}

    public void Disable()
    {
        foreach (Transform child in transform)
        {
            child.localScale = new Vector3(1f, 1f, 1f);
        }
        if (EventSystem != null)
        {
            EventSystem.SetSelectedGameObject(null);
            if (SelectedGameObject != null)
                SelectedGameObject.GetComponent<ButtonFunctions>().ResetButtonColors();
            SelectedGameObject = null;
            _buttonSelected = false;
        }
    }

    private void OnEnable()
    {
        if (EventSystem != null)
        {
            if (firstSelected != null)
                SelectedGameObject = firstSelected;
            EventSystem.SetSelectedGameObject(SelectedGameObject);
            if (SelectedGameObject != null)
                SelectedGameObject.GetComponent<ButtonFunctions>().OnSelect(null);
            _buttonSelected = true;
        }
    }
}
