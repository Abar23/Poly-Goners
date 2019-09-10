using InControl;

public class Controller : IController
{
    private ControllerActions controllerActions;
    
    public Controller(ControllerActions controllerActions)
    {
        this.controllerActions = controllerActions;
    }

    public InputDevice GetInputDevice()
    {
        return controllerActions.Device;
    }


    public ControllerActions GetControllerActions()
    {
        return controllerActions;
    }

    public void SetControllerActions(ControllerActions controllerActions)
    {
        this.controllerActions = controllerActions;
    }
}
