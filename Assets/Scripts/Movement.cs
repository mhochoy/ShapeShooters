using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Transform player;
    [Range(0,10)]
    [SerializeField] private float speed;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        if (_player && player == null) {
            player = _player.transform;
        }
    }

    public void Move(float x, float y) {
        Vector2 movement = new Vector2(x, y);

        rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }

    public void Move() {
        Vector2 relative = player.position - transform.position;
        float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
}
