﻿using InControl;

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

    public PlayerAction cheatRegeneration;

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

        cheatRegeneration = CreatePlayerAction("Cheat Regeneration");
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

        actions.action1.AddDefaultBinding(Key.Space);
        actions.action2.AddDefaultBinding(Key.LeftControl);
        actions.action3.AddDefaultBinding(Key.Key3); // Not used
        actions.action4.AddDefaultBinding(Key.Key4); // Not used

        actions.dPadUp.AddDefaultBinding(Key.UpArrow); 
        actions.dPadDown.AddDefaultBinding(Key.DownArrow);
        actions.dPadLeft.AddDefaultBinding(Key.Q);
        actions.dPadRight.AddDefaultBinding(Key.E);

        actions.leftBumper.AddDefaultBinding(Mouse.LeftButton);
        actions.rightBumper.AddDefaultBinding(Mouse.RightButton);

        actions.leftTrigger.AddDefaultBinding(Key.F); // Not used
        actions.rightTrigger.AddDefaultBinding(Key.G); // Not used

        actions.leftStickClick.AddDefaultBinding(Key.Tilde); // Not used, so it is put far away from the player
        actions.rightStickClick.AddDefaultBinding(Mouse.MiddleButton);

        actions.cheatRegeneration.AddDefaultBinding(Key.X);

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

        actions.start.AddDefaultBinding(InputControlType.Options);

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
