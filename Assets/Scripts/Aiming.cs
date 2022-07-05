using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aiming : MonoBehaviour
{
    
    Transform player;
    [SerializeField] private Transform pivot;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Aim(float x, float y) {
        if (x == 0.00f && y == 0.00f) {
            pivot.gameObject.SetActive(false);
        }
        else {
            pivot.gameObject.SetActive(true);
        }
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Aim() {
        Vector2 relative = player.position - transform.position;
        float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
