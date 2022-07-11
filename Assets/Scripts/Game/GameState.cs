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

    void Update()
    {
        bool PrizesAreCollected = prizes.TrueForAll( ( Prize prize ) => { return prize.collected; } );
        bool AreasIsCleared = areas.TrueForAll( ( Area area ) => { return area.cleared; } );
        bool LevelIsCleared = PrizesAreCollected && AreasIsCleared; 

        if (LevelIsCleared) {
            // End Level
            LevelStatus.enabled = true;
            Debug.Log("Level has been cleared!");
        }
        else {
            LevelStatus.enabled = false;
        }
        try {
            GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
            camera.m_Follow = player.transform;
        }
        catch (IndexOutOfRangeException) {
            Debug.Log("No Player Found.");
        }
    }
}
