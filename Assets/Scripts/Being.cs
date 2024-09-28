using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Health))]
public class Being : MonoBehaviour
{
    [SerializeField] bool AI;
    protected Aiming aiming;
    protected Movement movement;
    protected Guard guard;
    protected Health health;
    protected Shoot weapon;
    protected Rigidbody2D rb;
    protected bool block;
    protected bool shoot;
    
    // Start is called before the first frame update
    void Start()
    {
        GatherComponents();
    }

    void GatherComponents() {
        aiming = GetComponentInChildren<Aiming>();
        movement = GetComponent<Movement>();
        guard = GetComponentInChildren<Guard>();
        health = GetComponent<Health>();
        weapon = GetComponentInChildren<Shoot>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (health.dead) {
            DisableScripts();
            return;
        }
        HandleAimLook();

        if (AI) {
            float DistanceFromPlayer = 999;      
            if (aiming.LockedOntoPlayer()) { 
                RaycastHit2D hit = Physics2D.Raycast(weapon.GetFirePoint(), weapon.transform.up, DistanceFromPlayer, ~6);
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

    void HandleAimLook() {
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

    protected void FireInternal() {
        weapon.Fire();
    }

    public void Block(InputAction.CallbackContext context) {
        block = true;
        if (context.canceled) {
            block = false;
        }
    }

    protected virtual void OnCollisionStay2D(Collision2D col) {
        Health health;

        col.gameObject.TryGetComponent<Health>(out health);

        if (health && health.isDrawing) {
            health.Damage((int)rb.mass);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D trigger) {
        Prize prize;
        Being being;

        trigger.gameObject.TryGetComponent<Prize>(out prize);
        trigger.gameObject.TryGetComponent<Being>(out being);

        if (!AI) {
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

            if (being) {
                if (being.AI) {
                    being.SetTarget(this);
                }
            }
        }
    }

    public void SetTarget(Being b) {
        aiming.SetTarget(b);
    }

    public bool IsLockedOntoTarget() {
        if (aiming) {
            return aiming.LockedOntoPlayer();
        } else return false;
    }

    void DisableScripts() {
        if (aiming) {aiming.enabled = false;}
        if (movement) {movement.enabled = false;}
        if (weapon) {weapon.enabled = false;}
        if (guard) {guard.enabled = false;}
        health.enabled = false;
        this.enabled = false;
        gameObject.SetActive(false);
    }
}
