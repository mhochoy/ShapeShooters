using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    public bool isDrawing;
    public void Damage(int val) {
        if (health - val > 0) {
            health -= val;
        }
        else {
            Die();
        }
    }

    public void Die() {
        gameObject.SetActive(false);
    }
}
