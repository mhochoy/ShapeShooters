using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject ExplosionEffect;
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] List<AudioClip> HitSounds;
    SpriteRenderer Sprite;
    CircleCollider2D Collider;
    Rigidbody2D rb;
    AudioSource _Audio;
    GameObject shakeObj;
    Health health;
    void Start()
    {
        shakeObj = Camera.main.gameObject;
        Sprite = GetComponent<SpriteRenderer>();
        Collider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        _Audio = GetComponent<AudioSource>();
        TryGetComponent<Health>(out health);
    }

    void OnCollisionEnter2D(Collision2D col) {
        Health _health;
        Bullet _bullet;
        string collision_tag = col.transform.tag;
        col.gameObject.TryGetComponent<Health>(out _health);
        col.gameObject.TryGetComponent<Bullet>(out _bullet);

        if (_health) {
            bool BulletIsLessPowerful = health.health < _health.health;
            bool BulletIsHittingAPlayer = collision_tag == "Player";
            bool BulletIsHittingADrawBlock = collision_tag == "DefenseBlock";
            bool DisableConditionsAreMet = BulletIsLessPowerful || BulletIsHittingAPlayer || BulletIsHittingADrawBlock;
            PlayRandomHitSound();
            _health.Damage(damage, col.GetContact(0).point);
            if (DisableConditionsAreMet) {
                Disable();
            }
            shakeObj.SendMessage("TriggerShake");
        }
        else {
            Disable();
        }

        if (ExplosionEffect) {
            GameObject explosion = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        }
    }

    public void Disable() {
        Sprite.enabled = false;
        Collider.enabled = false;
        rb.simulated = false;
    }

    void PlayRandomHitSound() {
        int decision = Random.Range(0, HitSounds.Count);
        _Audio.clip = HitSounds[decision];
        _Audio.Play();
    }
}
