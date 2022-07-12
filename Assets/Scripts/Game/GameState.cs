using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;

public class GameState : MonoBehaviour
{
    public List<Area> areas;
    public List<Prize> prizes;
    public string NextScene;
    public CinemachineVirtualCamera camera;
    public TextMeshProUGUI LevelStatus;
    GameObject MainPlayer;
    bool LoadingNextScene;

    void Update()
    {
        
        bool PrizesAreCollected = prizes.TrueForAll( ( Prize prize ) => { return prize.collected; } );
        bool AreasIsCleared = areas.TrueForAll( ( Area area ) => { return area.cleared; } );
        bool LevelIsCleared = PrizesAreCollected && AreasIsCleared;

        try {

            MainPlayer = GameObject.FindGameObjectsWithTag("Player")[0];
            camera.m_Follow = MainPlayer.transform;
            if (HandleLevelStatus(LevelIsCleared) && !LoadingNextScene) {
                LoadingNextScene = true;
                Invoke("LoadNextScene", 4);
            }

        }
        catch (IndexOutOfRangeException) {

            Debug.Log("No Player Found.");

        }

    }

    bool HandleLevelStatus(bool levelIsCleared) {
        if (levelIsCleared) {
            // End Level
            LevelStatus.enabled = true;
            return true;
        }
        else {
            LevelStatus.enabled = false;
            return false;
        }
    }
    
    void LoadNextScene() {
        if (NextScene == "") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        SceneManager.LoadScene(NextScene);
    }
}
