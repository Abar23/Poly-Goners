using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour
{
    public EventSystem EventSystem;
    public GameObject SelectedGameObject;

    private bool _buttonSelected;
    private ControllerManager cm;

    private void Start()
    {
        cm = ControllerManager.GetInstance().GetComponent<ControllerManager>();
    }

    // Update is called once per frame
    void Update ()
	{
	    if (!cm.GetPlayerOneController().GetControllerActions().move.Y.Equals(0f) && _buttonSelected == false)
	    {
            EventSystem.SetSelectedGameObject(SelectedGameObject);
            _buttonSelected = true;
        }
	}

    private void OnDisable()
    {
        _buttonSelected = false;
    }
}
