using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float x;
    public float y;
    Transform player;
    [Range(0,50)]
    [SerializeField] private float speed;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        Vector2 movement = new Vector2(x, y);
        bool WeFoundAPlayerButItHasntBeenSetYet = _player && player == null;

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        if (WeFoundAPlayerButItHasntBeenSetYet) {
            player = _player.transform;
        }
    }

    public void Move(float x, float y) {
        Vector2 movement = new Vector2(x, y);
    }

    public void Move() {
        Vector2 relative = player.position - transform.position;
        float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
}
