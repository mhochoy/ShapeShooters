using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Guard : MonoBehaviour
{
    public GameObject ShieldObject;
    public GameObject DrawObject;
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
        weapon.SetActive(false);
        ShieldObject.SetActive(true);
    }

    void StopShield() {
        ShieldObject.SetActive(false);
        weapon.SetActive(true);
    }

    void Draw() {
        weapon.SetActive(false);
        GameObject shape = Instantiate(DrawObject, weapon.transform.position, weapon.transform.rotation);
    }

    void KillDraw() {
        weapon.SetActive(true);
    }
}
