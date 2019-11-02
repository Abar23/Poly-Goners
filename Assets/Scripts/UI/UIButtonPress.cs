using UnityEngine;
using UnityEngine.UI;
using InControl;

public class UIButtonPress : MonoBehaviour
{
    public int PlayerNumber;
    public string Action;
    public Image ButtonIcon;
    public Sprite ButtonDefaultSpritePS4;
    public Sprite ButtonPressedSpritePS4;
    public Sprite ButtonDefaultSpriteXbox;
    public Sprite ButtonPressedSpriteXbox;
    public Sprite ButtonDefaultSpritePC;
    public Sprite ButtonPressedSpritePC;

    private IController controller;

    void LateUpdate()
    {
        if (PlayerNumber == 1)
            controller = ControllerManager.GetInstance().GetPlayerOneController();
        else
            controller = ControllerManager.GetInstance().GetPlayerTwoController();

        if(controller is Controller)
        {
            if (controller.GetControllerActions().LastInputType == BindingSourceType.DeviceBindingSource)
            {
                if (controller.GetInputDevice().Name == "PlayStation 4 Controller")
                {
                    CheckInput(ButtonDefaultSpritePS4, ButtonPressedSpritePS4);
                }
                else
                {
                    CheckInput(ButtonDefaultSpriteXbox, ButtonPressedSpriteXbox);
                }
            }
            else if (controller.GetControllerActions().LastInputType == BindingSourceType.KeyBindingSource)
            {
                CheckInput(ButtonDefaultSpritePC, ButtonPressedSpritePC);
            }
        }
    }

    private void CheckInput(Sprite regular, Sprite pressed)
    {
        if (Action == "rightBumper")
        {
            if (controller.GetControllerActions().rightBumper.IsPressed)
            {
                ButtonIcon.sprite = pressed;
            }
            else
            {
                ButtonIcon.sprite = regular;
            }
        }
        else if (Action == "leftBumper")
        {
            if (controller.GetControllerActions().leftBumper.IsPressed)
            {
                ButtonIcon.sprite = pressed;
            }
            else
            {
                ButtonIcon.sprite = regular;
            }
        }
        else if (Action == "action4")
        {
            if (controller.GetControllerActions().action4.IsPressed)
            {
                ButtonIcon.sprite = pressed;
            }
            else
            {
                ButtonIcon.sprite = regular;
            }
        }
        else if (Action == "none")
        {
            ButtonIcon.sprite = regular;
        }

    }
}
