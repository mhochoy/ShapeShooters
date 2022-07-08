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
    
    public void Update() {
        last_fired += Time.deltaTime;
    }

    public void Fire() {
        if (last_fired > fireRate) {
            last_fired = 0;
            ShootBullet();
        }
    }

    public void FireBullet() {
        ShootBullet();
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

    public Vector2 GetFirePoint() {
        return FirePoint.position;
    }

    public Vector2 GetFireDirection() {
        return FirePoint.forward;
    }
}
