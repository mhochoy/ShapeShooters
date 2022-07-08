using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;

public class GameState : MonoBehaviour
{
    public List<Area> areas;
    public CinemachineVirtualCamera camera;
    public CinemachineVirtualCamera target_camera;
    public CinemachineTargetGroup TargetGroup;
    [Range(1, 10)]
    public float LookDistance;
    List<GameObject> AllEnemies;
    GameObject closest_enemy;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Area _area in areas) {    
            //     
        }
    }

    // Update is called once per frame
    void Update()
    {
        try {
            GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
            camera.m_Follow = player.transform;
        }
        catch (IndexOutOfRangeException) {
            Debug.Log("No Player Found.");
        }
    }
}
