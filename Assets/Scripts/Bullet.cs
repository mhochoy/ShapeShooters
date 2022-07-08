using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] List<AudioClip> HitSounds;
    SpriteRenderer Sprite;
    CircleCollider2D Collider;
    Rigidbody2D rb;
    AudioSource _Audio;
    GameObject shakeObj;
    void Start()
    {
        shakeObj = Camera.main.gameObject;
        Sprite = GetComponent<SpriteRenderer>();
        Collider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        _Audio = GetComponent<AudioSource>();
    }

    void Update() {       
    }

    void OnCollisionEnter2D(Collision2D col) {
        Health health;

        col.gameObject.TryGetComponent<Health>(out health);

        if (health) {
            int decision = Random.Range(0, HitSounds.Count);
            _Audio.clip = HitSounds[decision];
            _Audio.Play();
            health.Damage(damage, col.GetContact(0).point);
            shakeObj.SendMessage("TriggerShake");
        }
        
        Disable();
    }

    void Disable() {
        Sprite.enabled = false;
        Collider.enabled = false;
        rb.simulated = false;
    }
}
