using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aiming : MonoBehaviour
{
    
    protected Transform player;
    [SerializeField] private Transform pivot;
    [SerializeField] private float LockOnDistance;

    void Update() {
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        if (_player) {
            player = _player.transform;
        }
    }

    public void Aim(float x, float y) {
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Aim() {
        if (player) {
            Vector2 relative = player.position - transform.position;
            float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public bool LockedOntoPlayer() {
        // If player is within attacking distance of me, I am locked on
        if (player) {
            if (Vector2.Distance(transform.position, player.position) < LockOnDistance) return true;
            else return false;
        }
        else {
            return false;
        }
    }
}
