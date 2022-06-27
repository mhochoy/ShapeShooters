using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Range(0,10)]
    [SerializeField] private int speed;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movement = Gamepad.current.leftStick.ReadValue();

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        //transform.Translate((movement * speed) * Time.deltaTime);
    }
}
