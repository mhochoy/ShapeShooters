using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrizeType {
    Health,
    Powerup,
    MissionPrize,
}

public class Prize : MonoBehaviour
{
    public GameObject PrizeObject;
    public PrizeType type;
    public bool collected {get; private set;}
    public int value;
    AudioSource Audio;
    SpriteRenderer Renderer;
    BoxCollider2D Collider;

    void Start() {
        Audio = GetComponent<AudioSource>();
        Renderer = PrizeObject.GetComponent<SpriteRenderer>();
        Collider = GetComponent<BoxCollider2D>();
    }

    public void CollectPrize() {
        collected = true;
        Renderer.enabled = false;
        Collider.enabled = false;
        Audio.Play();
        PrizeObject.SetActive(false);
    }
}
