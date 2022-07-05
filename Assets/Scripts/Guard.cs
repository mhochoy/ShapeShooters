using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Guard : MonoBehaviour
{
    public GameObject ShieldObject;
    [SerializeField] private bool draw;
    [SerializeField] private GameObject weapon;

    public void Block(bool button_pressed) {
        if (button_pressed) {
            if (draw) {
                Draw();
            }
            else {
                SpawnShield();
            }
        }
        else {
            StopShield();
            KillDraw();
        }
    }

    void SpawnShield() {
        ShieldObject.SetActive(true);
        weapon.SetActive(false);
    }

    void StopShield() {
        ShieldObject.SetActive(false);
        weapon.SetActive(true);
    }

    void Draw() {
        ShieldObject.SetActive(true);
        weapon.SetActive(false);
        GameObject shape = Instantiate(ShieldObject, weapon.transform.position, weapon.transform.rotation);
    }

    void KillDraw() {
        ShieldObject.SetActive(false);
        weapon.SetActive(true);
    }
}
