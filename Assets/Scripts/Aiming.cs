using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aiming : MonoBehaviour
{
    public bool AI;
    public Transform player;
    [SerializeField] private Transform pivot;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!AI) {
            Vector2 aim = Gamepad.current.rightStick.ReadValue();
            Vector2 _aim = aim;

            if (_aim.x == 0.00f && _aim.y == 0.00f) {
                pivot.gameObject.SetActive(false);
            }
            else {
                pivot.gameObject.SetActive(true);
            }
            float angle = Mathf.Atan2(_aim.y, _aim.x) * Mathf.Rad2Deg - 90f;

            //rb.rotation = angle;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else {
            Vector2 relative = player.position - transform.position;
            float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
            //transform.Rotate(0, 0, angle);
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
    }
}
