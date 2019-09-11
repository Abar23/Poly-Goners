using System.Collections.Generic;
using InControl;

public interface IController
{
    InputDevice GetInputDevice();

    ControllerActions GetControllerActions();

    void SetControllerActions(ControllerActions controllerActions);
}
