using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class Shoot : MonoBehaviour
{
    public bool Auto;
    public bool AI;
    [SerializeField] float fireRate;
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform FirePoint;
    [SerializeField] int speed;
    float last_fired;
    
    // Update is called once per frame
    void Update()
    {
        if (!AI) {
            if (Auto) {
                float shoot = Gamepad.current.rightShoulder.ReadValue();

                last_fired += Time.deltaTime;
                
                if (last_fired > fireRate && shoot > 0f) {
                    last_fired = 0;
                    ShootBullet();
                }
            }
            else {
                if (Gamepad.current.rightShoulder.wasPressedThisFrame) {
                    ShootBullet();
                }
            }
        }
        else {
            last_fired += Time.deltaTime;
                
            if (last_fired > fireRate) {
                last_fired = 0;
                ShootBullet();
            }
        }
    }

    void ShootBullet() {
        GameObject bullet;
        if (FirePoint) {
            bullet = Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
        }
        else {
            bullet = Instantiate(Bullet, transform.position, transform.rotation);
        }
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.up * speed, ForceMode2D.Impulse);
    }
}
