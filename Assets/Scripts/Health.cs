using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    public bool isDrawing;
    public bool Indestructible;
    [SerializeField] GameObject HitEffectObj;
    [SerializeField] GameObject DeathEffectObj;
    ParticleSystem HitEffect;
    ParticleSystem DeathEffect;

    void Start() {
        if (HitEffectObj) {
            HitEffectObj.TryGetComponent<ParticleSystem>(out HitEffect);
        }
        if (DeathEffectObj) {
            DeathEffectObj.TryGetComponent<ParticleSystem>(out DeathEffect);
        }
    }

    public void Give(int val) {
        health += val;
    }

    public void Damage(int val) {
        if (Indestructible) {
            return;
        }
        if (health - val > 0) {
            health -= val;
        }
        else {
            Die();
        }
    }

    public void Damage(int val, Vector2 point) {
        if (Indestructible) {
            SpawnHitEffect(point);
            return;
        }
        if (health - val > 0) {
            SpawnHitEffect(point);
            health -= val;
        }
        else {
            Die();
        }
    }

    public void SpawnHitEffect(Vector2 hit_point) {
        if (HitEffect) {
            HitEffect.transform.position = hit_point;
            HitEffect.transform.rotation = Quaternion.identity;

            HitEffect.Play();
        }
    }

    public void Die() {
        if (DeathEffect) {
            GameObject _effect = Instantiate(DeathEffectObj, transform.position, transform.rotation);
        }
        if (isDrawing) {
            Bullet bullet;
            TryGetComponent<Bullet>(out bullet);

            if (bullet) {
                bullet.Disable();
            }
            return;
        }
        gameObject.SetActive(false);
    }
}
