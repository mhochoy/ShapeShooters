using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Guard : MonoBehaviour
{
    public GameObject ShieldObject;
    [SerializeField] private bool draw;
    [SerializeField] private GameObject weapon;

    void Update()
    {
        float guard = Gamepad.current.leftShoulder.ReadValue();
        if (!draw) {
            if (guard > 0.00f) {
                ShieldObject.SetActive(true);
                weapon.SetActive(false);
            }
            else {
                ShieldObject.SetActive(false);
                weapon.SetActive(true);
            }
        }
        else {
            if (guard > 0.00f) {
                ShieldObject.SetActive(true);
                weapon.SetActive(false);
                GameObject shape = Instantiate(ShieldObject, weapon.transform.position, weapon.transform.rotation);
            }
            else {
                ShieldObject.SetActive(false);
                weapon.SetActive(true);
                // Nothing
            }
        }
    }
}
