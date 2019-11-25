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

        cm = ControllerManager.GetInstance().GetComponent<ControllerManager>();
        firstSelected = SelectedGameObject;
    }

    // Update is called once per frame
    void Update ()
	{
	    if (!cm.GetPlayerOneController().GetControllerActions().move.Y.Equals(0f) && _buttonSelected == false)
	    {
            EventSystem.SetSelectedGameObject(SelectedGameObject);
            _buttonSelected = true;
        }

        if (PreviousMenu != null && cm.GetPlayerOneController().GetControllerActions().action2.WasPressed)
        {
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

    private void OnDisable()
    {
        _buttonSelected = false;
    }

    private void OnEnable()
    {
        SelectedGameObject = firstSelected;
        if (EventSystem != null)
            EventSystem.SetSelectedGameObject(SelectedGameObject);
        _buttonSelected = true;
    }
}
