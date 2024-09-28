using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    public bool isDrawing;
    public bool Indestructible;
    public SpriteRenderer Renderer;
    public Color HitColor;
    public bool dead;
    [SerializeField] GameObject HitEffectObj;
    [SerializeField] GameObject DeathEffectObj;
    ParticleSystem HitEffect;
    ParticleSystem DeathEffect;
    Color originalColor;

    void Start() {
        if (!Renderer) {
            TryGetComponent<SpriteRenderer>(out Renderer);
        }
        originalColor = Renderer.color;
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
            //StartCoroutine(DamageEffectSequence(Renderer, HitColor, .1f, 0));
            health -= val;
        }
        else {
            Die();
        }
    }

    public void Damage(int val, Vector2 point) {
        Damage(val);
        SpawnHitEffect(point);
    }

    public void FlashDamage(SpriteRenderer renderer) {
        if (!gameObject.activeInHierarchy) {
            return;
        }
        if (Renderer) {
            StartCoroutine(DamageEffectSequence(Renderer, HitColor, .1f, 0));
        }
        else {
            StartCoroutine(DamageEffectSequence(renderer, HitColor, .1f, 0));
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
        dead = true;
        SendMessage("DisableScripts");
    }

    IEnumerator DamageEffectSequence(SpriteRenderer sr, Color dmgColor, float duration, float delay)
    {
        // save origin color
        Color originColor = sr.color;

        // tint the sprite with damage color
        sr.color = dmgColor;

        // you can delay the animation
        yield return new WaitForSeconds(delay);

        // lerp animation with given duration in seconds
        for (float t = 0; t < 1.0f; t += Time.deltaTime/duration)
        {
            sr.color = Color.Lerp(dmgColor, originalColor , t);

            yield return null;
        }

        // restore origin color
        sr.color = originalColor;
    }
}
