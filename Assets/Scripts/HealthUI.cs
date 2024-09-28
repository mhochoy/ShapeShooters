using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] GameObject[] players;
    [SerializeField] Vector3 offset;
    [SerializeField] GameObject canvasObj;
    Canvas canvas;
    [SerializeField] GameObject HealthComponent;
    Dictionary<Player, Health> Healths = new Dictionary<Player, Health>();
    Dictionary<GameObject, Player> healthBars = new Dictionary<GameObject, Player>();

    public void Start() {
        canvasObj = GameObject.FindGameObjectWithTag("Canvas");
        canvasObj.TryGetComponent<Canvas>(out canvas);
    }

    public static Vector2 WorldToCanvasPoint(Vector3 worldPoint, Canvas _canvas) {
        Camera c                    = Camera.main;
        // Used GetComponent for clarity, but you'll want to cache this rect transform in your actual production code for performance
        Vector2 canvasSize          = _canvas.GetComponent<RectTransform>().sizeDelta;

        Vector3 screenPoint         = c.WorldToViewportPoint(worldPoint);
        Vector2 screenPoint2D       = new Vector2(screenPoint.x, screenPoint.y);

        float canvasPointX          = screenPoint2D.x * canvasSize.x - 0.5f * canvasSize.x;
        float canvasPointY          = screenPoint2D.y * canvasSize.y - 0.5f * canvasSize.y;

        return new Vector2(canvasPointX, canvasPointY);
    }



// This update loop will make a canvas element track a world transform
public void Update() {
        try {
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players) {
                Player ps = player.GetComponent<Player>(); // Player Script
                Health health = player.GetComponent<Health>();

                if (!Healths.ContainsKey(ps) && !Healths.ContainsValue(health)) {
                    Healths.Add(ps, health);
                }
            }

            foreach (Player p in Healths.Keys) {
                if (!healthBars.ContainsValue(p)) {
                    GameObject healthbar = Instantiate(HealthComponent, canvasObj.transform);
                    healthBars.Add(healthbar, p);
                }
            }

            foreach (GameObject bar in healthBars.Keys) {
                Slider healthbar = bar.GetComponent<Slider>();
                Player player;
                Health health;

                healthBars.TryGetValue(bar, out player);
                Healths.TryGetValue(player, out health);
                
                if (!health.dead) {
                    bar.transform.position = WorldToCanvasPoint(player.transform.position + offset, canvas);
                    healthbar.value = health ? health.health : 0f;
                }
                else {
                    bar.SetActive(false);
                    Healths.Remove(player);
                    healthBars.Remove(bar);
                }
                
            }
        }
        catch (InvalidOperationException exception) {
            Debug.Log($"Exception Caught: {exception.Message}");
        }
    }
}
