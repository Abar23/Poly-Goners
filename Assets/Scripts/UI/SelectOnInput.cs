using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour
{
    public EventSystem EventSystem;
    public GameObject SelectedGameObject;

    private bool _buttonSelected;
	
	// Update is called once per frame
	void Update ()
	{
	    if (!Input.GetAxisRaw("Vertical").Equals(0.0f) && _buttonSelected == false)
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
