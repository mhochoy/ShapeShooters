using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Linq;

public class Player : Being
{
    static Player[] instances;
    public static Player[] allPlayers {
        get {
            if (instances == null) {
                instances = FindObjectsOfType<Player>();

                return instances;
            }
            else {
                return instances;
            }
        }
    }
    PlayerInput playerInput;
    InputManager input;
    
    void Awake()
    {
        if (!instances.Contains(this)) {
            instances.Append(this);
        }
        TryGetComponent<PlayerInput>(out playerInput);
        TryGetComponent<InputManager>(out input);
        if (!playerInput) {
            ToggleInput(true);
        }
        else {
            ToggleInput(false);
        }
    }

    void ToggleInput(bool decision) {
        if (input) {
            input.enabled = decision;
        }
    }

    protected override void Update() {
        bool PlayerIsUsingRawInput = input.enabled;
        bool PlayerIsUsingInputManager = !input.enabled;

        base.Update();

        if (PlayerIsUsingInputManager) {
            if (shoot) {
                base.FireInternal();
            }
            if (block) {
                base.guard.Block(true);
            }
            else {
                base.guard.Block(false);
            }
        }
        if (PlayerIsUsingRawInput) {
            var _movement = input.MoveAction.ReadValue<Vector2>();
            var _look = input.LookAction.ReadValue<Vector2>();
            bool WeaponIsSemiAutomatic = input.FireAction.WasPressedThisFrame() && !weapon.Auto;
            bool WeaponIsAutomatic = input.FireAction.IsPressed() && weapon.Auto;
            // Move + Look
            Move(_movement);
            Look(_look);
            // Blocking
            guard.Block(input.BlockAction.IsPressed());
            // Firing
            if (WeaponIsSemiAutomatic) {
                weapon.FireBullet();
            }
            else if (WeaponIsAutomatic) {
                weapon.Fire();
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D trigger)
    {
        base.OnTriggerEnter2D(trigger);

        if (trigger.gameObject.CompareTag("Area")) {
            Area area = trigger.gameObject.GetComponent<Area>();

            area.Activate();
        }
    }

    void OnTriggerStay2D(Collider2D trigger)
    {
        base.OnTriggerEnter2D(trigger);

        if (trigger.gameObject.CompareTag("Area")) {
            Area area = trigger.gameObject.GetComponent<Area>();

            area.Activate();
        }
    }

    void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("Area")) {
            Area area = trigger.gameObject.GetComponent<Area>();

            area.Deactivate();
        }
    }

    void OnDisable() {
        playerInput.enabled = false;
        gameObject.SetActive(false);
    }
}
