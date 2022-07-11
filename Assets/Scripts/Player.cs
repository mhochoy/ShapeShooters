using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    public bool AI;
    Health health;
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
        TryGetComponent<Health>(out health);
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
        bool PlayerIsUsingRawInput = !AI && input.enabled;
        bool PlayerIsUsingInputManager = !AI && !input.enabled;
        if (tag == "Player") {
            bool LookingLeft = aiming.transform.rotation.z > 0.00f && aiming.transform.rotation.z < 180.00;
            bool LookingRight = aiming.transform.rotation.z < 0.00f && aiming.transform.rotation.z > -180.00f;
            if (LookingLeft) {
                aiming.transform.localScale = new Vector2(-1, 1);
            }
            else if (LookingRight) {
                aiming.transform.localScale = new Vector2(1, 1);
            }
            else {
                // Nothing
            }
        }
        if (PlayerIsUsingInputManager) {
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
        if (PlayerIsUsingRawInput) {
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
            bool ThereIsNoInput = (_movement.x == 0f && _movement.y == 0f);
            if (ThereIsNoInput) {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            movement.x = _movement.x;
            movement.y = _movement.y;
        }
    }

    public void Move(Vector2 _movement) 
    {
        if (movement) {
            movement.x = _movement.x;
            movement.y = _movement.y;
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
        Prize prize;

        trigger.gameObject.TryGetComponent<Prize>(out prize);

        if (prize) {
            switch (prize.type) {
                case PrizeType.Health:
                    health.Give(prize.value);
                    prize.CollectPrize();
                    break;
                case PrizeType.Powerup:
                    break;
                default:
                    prize.CollectPrize();
                    break;
            }
        }
    }

    void OnDisable() {
        if (!AI) {
            playerInput.enabled = false;
        }
    }
}
