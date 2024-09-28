using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aiming : MonoBehaviour
{
    protected Being Target;
    [SerializeField] private float LockOnDistance;

    public void Aim(float x, float y) {
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Aim() {
        if (Target) {
            Vector2 relative = Target.transform.position - transform.position;
            float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public bool LockedOntoPlayer() {
        // If player is within attacking distance of me, I am locked on
        if (Target) {
            if (Vector2.Distance(transform.position, Target.transform.position) < LockOnDistance) return true;
            else return false;
        }
        else {
            return false;
        }
    }

    public void SetTarget(Being b) {
        Target = b;
    }
}
