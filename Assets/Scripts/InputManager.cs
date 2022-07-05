using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputAction MoveAction;
    public InputAction LookAction;
    public InputAction BlockAction;
    public InputAction FireAction;

    void Start()
    {
        MoveAction = new InputAction(
            name: "Move",
            binding: "<Gamepad>/leftStick"
        );
        LookAction = new InputAction(
            name: "Look",
            binding: "<Gamepad>/rightStick"
        );

        MoveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        LookAction.AddBinding("<Mouse>/delta");

        BlockAction = new InputAction(
            type: InputActionType.Button,
            binding: "<Gamepad>/leftShoulder",
            interactions: "press(behavior=1)"
        );
        FireAction = new InputAction(
            type: InputActionType.Button,
            binding: "<Gamepad>/rightShoulder",
            interactions: "press(behavior=1)"
        );
        MoveAction.Enable();
        LookAction.Enable();
        BlockAction.Enable();
        FireAction.Enable();
    }
}
