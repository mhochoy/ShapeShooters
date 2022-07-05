using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] float offset;
    [SerializeField] GameObject canvasObj;
    Canvas canvas;
    [SerializeField] GameObject HealthComponent;
    Health _Health;
    GameObject healthRef;

    public void Start() {
        canvasObj = GameObject.FindGameObjectWithTag("Canvas");
        canvasObj.TryGetComponent<Canvas>(out canvas);
        _Health = GetComponent<Health>();

        healthRef = Instantiate(HealthComponent, canvasObj.transform);
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
    Slider _healthbar = healthRef.GetComponent<Slider>();

    _healthbar.value = _Health.health;
    if (HealthComponent != null) {
        HealthComponent.transform.position = WorldToCanvasPoint(transform.position + Vector3.up*offset, canvas);
    }
}
}
