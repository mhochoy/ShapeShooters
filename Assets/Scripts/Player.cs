using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    public bool AI;
    PlayerInput playerInput;
    InputManager input;
    Movement movement;
    Guard guard;
    Shoot weapon;
    Aiming aiming;
    Rigidbody2D rb;
    bool block;
    bool shoot;
    float area_zoom;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<PlayerInput>(out playerInput);
        TryGetComponent<InputManager>(out input);
        if (!playerInput) {
            ToggleInput(true);
        }
        else {
            ToggleInput(false);
        }
        TryGetComponent<Movement>(out movement);
        aiming = GetComponentInChildren<Aiming>();
        guard = GetComponentInChildren<Guard>();
        weapon = GetComponentInChildren<Shoot>();
        TryGetComponent<Rigidbody2D>(out rb);
        rb = GetComponent<Rigidbody2D>();
    }

    void ToggleInput(bool decision) {
        if (input) {
            input.enabled = decision;
        }
    }

    public void Update() {
        if (tag == "Player") {
            if (aiming.transform.rotation.z > 0.00f && aiming.transform.rotation.z < 180.00f) {
                aiming.transform.localScale = new Vector2(-1, 1);
            }
            else if (aiming.transform.rotation.z < 0.00f && aiming.transform.rotation.z > -180.00f) {
                aiming.transform.localScale = new Vector2(1, 1);
            }
            else {
                // Nothing
            }
        }
        if (!AI && !input.enabled) {
            if (shoot) {
                FireInternal();
            }
            if (block) {
                guard.Block(true);
            }
            else {
                guard.Block(false);
            }
        }
        if (!AI && input.enabled) {
            var _movement = input.MoveAction.ReadValue<Vector2>();
            var _look = input.LookAction.ReadValue<Vector2>();
            // Move + Look
            Move(_movement);
            Look(_look);
            // Blocking
            guard.Block(input.BlockAction.IsPressed());
            // Firing
            bool WeaponIsSemiAutomatic = input.FireAction.WasPressedThisFrame() && !weapon.Auto;
            bool WeaponIsAutomatic = input.FireAction.IsPressed() && weapon.Auto;
            if (WeaponIsSemiAutomatic) {
                weapon.FireBullet();
            }
            else if (WeaponIsAutomatic) {
                weapon.Fire();
            }
        }
        if (AI) {
            Player player;
            float DistanceFromPlayer = 999;      
            if (aiming.LockedOntoPlayer()) { 
                RaycastHit2D hit = Physics2D.Raycast(weapon.GetFirePoint(), weapon.transform.up, DistanceFromPlayer);
                DistanceFromPlayer = Vector2.Distance(weapon.GetFirePoint(), hit.point); 
                if (hit) {
                    if (hit.transform.gameObject.tag == "Player") { 
                        weapon.Fire();
                    }
                }   
                Move();
                Look();
            }
        }
    }

    public void Move(InputAction.CallbackContext context) {
        var _movement = context.ReadValue<Vector2>();
        if (movement) {
            movement.Move(_movement.x, _movement.y);
        }
    }

    public void Move(Vector2 _movement) 
    {
        if (movement) {
            movement.Move(_movement.x, _movement.y);
        }
        
    }

    public void Move() {
        if (movement) {
            movement.Move();
        }
    }

    public void Look(InputAction.CallbackContext context) {
        var _look = context.ReadValue<Vector2>();
        if (aiming) {
            aiming.Aim(_look.x, _look.y);
        }
    }

    public void Look(Vector2 _look) {
        if (aiming) {
            aiming.Aim(_look.x, _look.y);
        }
    }

    public void Look() {
        if (aiming) {
            aiming.Aim();
        }
    }

    public void Fire(InputAction.CallbackContext context) {
        shoot = true;
        if (context.canceled) {
            shoot = false;
        }
    }

    void FireInternal() {
        weapon.Fire();
    }

    public void Block(InputAction.CallbackContext context) {
        block = true;
        if (context.canceled) {
            block = false;
        }
    }

    void OnCollisionStay2D(Collision2D col) {
        Health health;

        col.gameObject.TryGetComponent<Health>(out health);

        if (health && health.isDrawing) {
            health.Damage((int)rb.mass);
        }
    }

    void OnTriggerEnter2D(Collider2D trigger) {
        AreaTransitions areaTransitions;
        if (trigger.CompareTag("Area")) {
            trigger.TryGetComponent<AreaTransitions>(out areaTransitions);

            if (areaTransitions) {
                area_zoom = areaTransitions.AreaZoomSize;
            }
        }
    }

    void OnDisable() {
        if (!AI) {
            playerInput.enabled = false;
        }
    }
}
