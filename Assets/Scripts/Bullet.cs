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
    Collider2D Collider;
    Rigidbody2D rb;
    AudioSource _Audio;
    Health health;
    float original_volume;
    float original_pitch;
    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
        Collider = GetComponent<CircleCollider2D>();
        if (!Collider) {
            Collider = GetComponent<BoxCollider2D>();
        }
        rb = GetComponent<Rigidbody2D>();
        _Audio = GetComponent<AudioSource>();
        original_volume = _Audio.volume;
        original_pitch = _Audio.pitch;
        TryGetComponent<Health>(out health);
    }

    void OnCollisionEnter2D(Collision2D col) {
        Health _health;
        Bullet _bullet;
        SpriteRenderer _renderer;
        Rigidbody2D _rb;
        string collision_tag = col.transform.tag;
        col.gameObject.TryGetComponent<Health>(out _health);
        col.gameObject.TryGetComponent<Bullet>(out _bullet);
        col.gameObject.TryGetComponent<SpriteRenderer>(out _renderer);
        col.gameObject.TryGetComponent<Rigidbody2D>(out _rb);

        if (_health) {
            bool BulletIsLessPowerful = health.health < _health.health;
            bool BulletIsHittingAPlayer = collision_tag == "Player";
            bool BulletIsHittingADrawBlock = collision_tag == "DefenseBlock";
            bool BulletIsLessHeavy = rb.mass <= _rb.mass;
            bool DisableConditionsAreMet = BulletIsLessPowerful || BulletIsHittingAPlayer || BulletIsHittingADrawBlock || BulletIsLessHeavy;
            PlayRandomHitSound();
            _health.Damage(damage, col.GetContact(0).point);
            if (_renderer) {
                _health.FlashDamage(_renderer);
            }
            else {
                _health.FlashDamage(_health.Renderer);
            }
            if (DisableConditionsAreMet) {
                Disable();
            }
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
        _Audio.pitch = Random.Range(original_pitch, original_pitch + .25f);
        _Audio.volume = Random.Range(original_volume-.075f, original_volume+.25f);
        _Audio.Play();
    }
}
