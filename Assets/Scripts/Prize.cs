using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PrizeType {
    Health,
    Powerup,
    MissionPrize,
}

public class Prize : MonoBehaviour
{
    
    [SerializeField] static List<Prize> instances = new List<Prize>();
    public static List<Prize> Instances {
        get {
            if (instances == null) {
                instances = FindObjectsOfType<Prize>().ToList();

                return instances;
            } 
            else {
                return instances;
            }
        }
    }
    public GameObject PrizeObject;
    public PrizeType type;
    public bool collected {get; private set;}
    public int value;
    AudioSource Audio;
    SpriteRenderer Renderer;
    BoxCollider2D Collider;

    void Awake() {
        if (!instances.Contains(this)) {
            instances.Append(this);
        }
        Audio = GetComponent<AudioSource>();
        Renderer = PrizeObject.GetComponent<SpriteRenderer>();
        Collider = GetComponent<BoxCollider2D>();
    }

    public static bool AllInstancesCollected() {
        return Instances.All(prize => prize.collected);
    }

    public void CollectPrize() {
        collected = true;
        Renderer.enabled = false;
        Collider.enabled = false;
        Audio.Play();
        PrizeObject.SetActive(false);
    }
}
