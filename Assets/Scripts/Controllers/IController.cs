using System.Collections.Generic;
using InControl;

public interface IController
{
    InputDevice GetInputDevice();

    ControllerActions GetControllerActions();

    bool isUsingKeyboard();

    void SetControllerActions(ControllerActions controllerActions);
}
