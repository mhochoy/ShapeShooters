using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rainbow : MonoBehaviour
{
    public float Speed = 1;
    private SpriteRenderer rend;
 
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }
 
    void Update()
    {
        rend.material.SetColor("_Color", HSBColor.ToColor(new HSBColor( Mathf.PingPong(Time.time * Speed, 1), 1, 1)));
    }
}
