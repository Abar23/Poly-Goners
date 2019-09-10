using UnityEngine;
using InControl;

public class ControllerManager : AbstractSingleton<Component>
{
    private IController playerOneController;
    private IController playerTwoController;

    private void OnEnable()
    {
        ControllerActions keyboardListener = ControllerActions.CreateWithKeyBoardBindings();
        playerOneController = new Controller(keyboardListener);

        playerTwoController = new NullController();

        InputManager.OnDeviceAttached += AttachController;
        InputManager.OnDeviceDetached += DetachController;
    }

    private void OnDisable()
    {
        InputManager.OnDeviceAttached -= AttachController;
        InputManager.OnDeviceDetached -= DetachController;
    }

    private void DestroyControllerActions(IController controller)
    {
        controller.GetControllerActions().Destroy();
    }

    private void AttachController(InputDevice device)
    {
        if (!IsControllerAttached(device))
        {
            if(playerOneController.GetInputDevice() == null)
            {
                DestroyControllerActions(playerOneController);

                ControllerActions joystickListener = ControllerActions.CreateWithJoystickBindings();
                joystickListener.Device = device;
                playerOneController.SetControllerActions(joystickListener);
            }
            else if(playerTwoController.GetInputDevice() == null)
            {
                ControllerActions joystickListener = ControllerActions.CreateWithJoystickBindings();
                joystickListener.Device = device;
                playerTwoController = new Controller(joystickListener);
            }
        }
    }

    private void DetachController(InputDevice device)
    {
        if (playerOneController.GetInputDevice() == device)
        {
            DestroyControllerActions(playerOneController);

            ControllerActions keyboardListener = ControllerActions.CreateWithKeyBoardBindings();
            playerOneController.SetControllerActions(keyboardListener);
        }
        else if (playerTwoController.GetInputDevice() == device)
        {
            DestroyControllerActions(playerTwoController);

            playerTwoController = new NullController();
        }
    }

    private bool IsControllerAttached(InputDevice device)
    {
        bool inputDeviceIsAttached = false;

        InputDevice playerOneInputDevice = playerOneController.GetInputDevice();
        InputDevice playerTwoInputDevice = playerTwoController.GetInputDevice();
        if(playerOneInputDevice == device || playerTwoInputDevice == device)
        {
            inputDeviceIsAttached = true;
        }

        return inputDeviceIsAttached;
    }

    public IController GetPlayerOneController()
    {
        return playerOneController;
    }

    public IController GetPlayerTwoController()
    {
        return playerTwoController;
    }
}
