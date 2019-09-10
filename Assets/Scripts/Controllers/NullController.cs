using System.Collections.Generic;
using InControl;

public class NullController : IController
{
    public InputDevice GetInputDevice()
    {
        return null;
    }

    public ControllerActions GetControllerActions()
    {
        return null;
    }

    public void SetControllerActions(ControllerActions controllerActions)
    {

    }
}
