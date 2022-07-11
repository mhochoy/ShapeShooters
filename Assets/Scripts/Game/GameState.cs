using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;
using TMPro;

public class GameState : MonoBehaviour
{
    public List<Area> areas;
    public List<Prize> prizes;
    public CinemachineVirtualCamera camera;
    public TextMeshProUGUI LevelStatus;
    public GameObject MainPlayer;

    void Update()
    {
        
        bool PrizesAreCollected = prizes.TrueForAll( ( Prize prize ) => { return prize.collected; } );
        bool AreasIsCleared = areas.TrueForAll( ( Area area ) => { return area.cleared; } );
        bool LevelIsCleared = PrizesAreCollected && AreasIsCleared;

        HandleLevelStatus(LevelIsCleared);

        try {

            MainPlayer = GameObject.FindGameObjectsWithTag("Player")[0];
            camera.m_Follow = MainPlayer.transform;
            
        }
        catch (IndexOutOfRangeException) {

            Debug.Log("No Player Found.");

        }

    }

    void HandleLevelStatus(bool levelIsCleared) {
        if (levelIsCleared) {
            // End Level
            LevelStatus.enabled = true;
        }
        else {
            LevelStatus.enabled = false;
        }
    }
}
