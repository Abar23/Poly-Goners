using InControl;

public class ControllerActions : PlayerActionSet
{
    public PlayerAction moveUp;
    public PlayerAction moveDown;
    public PlayerAction moveLeft;
    public PlayerAction moveRight;
    public PlayerTwoAxisAction move;

    public PlayerAction lookUp;
    public PlayerAction lookDown;
    public PlayerAction lookLeft;
    public PlayerAction lookRight;
    public PlayerTwoAxisAction look;

    public PlayerAction start;

    public PlayerAction action1;
    public PlayerAction action2;
    public PlayerAction action3;
    public PlayerAction action4;

    public PlayerAction dPadUp;
    public PlayerAction dPadDown;
    public PlayerAction dPadLeft;
    public PlayerAction dPadRight;

    public PlayerAction leftBumper;
    public PlayerAction rightBumper;
 
    public PlayerAction leftTrigger;
    public PlayerAction rightTrigger;

    public PlayerAction leftStickClick;
    public PlayerAction rightStickClick;

    public ControllerActions()
    {

        moveUp = CreatePlayerAction("Move Up");
        moveDown = CreatePlayerAction("Move Down");
        moveLeft = CreatePlayerAction("Move Left");
        moveRight = CreatePlayerAction("Move Right");
        move = CreateTwoAxisPlayerAction(moveLeft, moveRight, moveDown, moveUp);
        
        lookUp = CreatePlayerAction("Look Up");
        lookDown = CreatePlayerAction("Look Down");
        lookLeft = CreatePlayerAction("Look Left");
        lookRight = CreatePlayerAction("Look Right");
        look = CreateTwoAxisPlayerAction(lookLeft, lookRight, lookDown, lookUp);

        start = CreatePlayerAction("Start");

        action1 = CreatePlayerAction("Action 1");
        action2 = CreatePlayerAction("Action 2");
        action3 = CreatePlayerAction("Action 3");
        action4 = CreatePlayerAction("Action 4");

        dPadUp = CreatePlayerAction("DPad Up");
        dPadDown = CreatePlayerAction("Dapd Down");
        dPadLeft = CreatePlayerAction("Dapd Left");
        dPadRight = CreatePlayerAction("Dapd Right");

        leftBumper = CreatePlayerAction("Left Bumper");
        rightBumper = CreatePlayerAction("Right Bumper");

        leftTrigger = CreatePlayerAction("Left Trigger");
        rightTrigger = CreatePlayerAction("Right Trigger");

        leftStickClick = CreatePlayerAction("Left Stick Click");
        rightStickClick = CreatePlayerAction("Right Stick Click");
    }

    public static ControllerActions CreateWithKeyBoardBindings()
    {
        ControllerActions actions = new ControllerActions();

        actions.moveUp.AddDefaultBinding(Key.W);
        actions.moveDown.AddDefaultBinding(Key.S);
        actions.moveLeft.AddDefaultBinding(Key.A);
        actions.moveRight.AddDefaultBinding(Key.D);

        actions.lookUp.AddDefaultBinding(Mouse.PositiveY);
        actions.lookDown.AddDefaultBinding(Mouse.NegativeY);
        actions.lookLeft.AddDefaultBinding(Mouse.NegativeX);
        actions.lookRight.AddDefaultBinding(Mouse.PositiveX);

        actions.start.AddDefaultBinding(Key.Escape);

        actions.action1.AddDefaultBinding(Key.Key1);
        actions.action2.AddDefaultBinding(Key.Key2);
        actions.action3.AddDefaultBinding(Key.Key3);
        actions.action4.AddDefaultBinding(Key.Key4);

        actions.dPadUp.AddDefaultBinding(Key.UpArrow);
        actions.dPadDown.AddDefaultBinding(Key.DownArrow);
        actions.dPadLeft.AddDefaultBinding(Key.LeftArrow);
        actions.dPadRight.AddDefaultBinding(Key.RightArrow);

        actions.leftBumper.AddDefaultBinding(Key.R);
        actions.rightBumper.AddDefaultBinding(Key.T);

        actions.leftTrigger.AddDefaultBinding(Key.F);
        actions.rightTrigger.AddDefaultBinding(Key.G);

        actions.leftStickClick.AddDefaultBinding(Key.Q);
        actions.rightStickClick.AddDefaultBinding(Key.E);

        return actions;
    }

    public static ControllerActions CreateWithJoystickBindings()
    {
        ControllerActions actions = new ControllerActions();

        actions.moveUp.AddDefaultBinding(InputControlType.LeftStickUp);
        actions.moveDown.AddDefaultBinding(InputControlType.LeftStickDown);
        actions.moveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
        actions.moveRight.AddDefaultBinding(InputControlType.LeftStickRight);

        actions.lookUp.AddDefaultBinding(InputControlType.RightStickUp);
        actions.lookDown.AddDefaultBinding(InputControlType.RightStickDown);
        actions.lookLeft.AddDefaultBinding(InputControlType.RightStickLeft);
        actions.lookRight.AddDefaultBinding(InputControlType.RightStickRight);

        actions.start.AddDefaultBinding(InputControlType.Start);

        actions.action1.AddDefaultBinding(InputControlType.Action1);
        actions.action2.AddDefaultBinding(InputControlType.Action2);
        actions.action3.AddDefaultBinding(InputControlType.Action3);
        actions.action4.AddDefaultBinding(InputControlType.Action4);

        actions.dPadUp.AddDefaultBinding(InputControlType.DPadUp);
        actions.dPadDown.AddDefaultBinding(InputControlType.DPadDown);
        actions.dPadLeft.AddDefaultBinding(InputControlType.DPadLeft);
        actions.dPadRight.AddDefaultBinding(InputControlType.DPadRight);

        actions.leftBumper.AddDefaultBinding(InputControlType.LeftBumper);
        actions.rightBumper.AddDefaultBinding(InputControlType.RightBumper);

        actions.leftTrigger.AddDefaultBinding(InputControlType.LeftTrigger);
        actions.rightTrigger.AddDefaultBinding(InputControlType.RightTrigger);

        actions.leftStickClick.AddDefaultBinding(InputControlType.LeftStickButton);
        actions.rightStickClick.AddDefaultBinding(InputControlType.RightStickButton);

        return actions;
    }
}
