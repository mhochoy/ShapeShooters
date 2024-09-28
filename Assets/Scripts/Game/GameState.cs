using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set;}
    public Area[] Areas;
    public string NextScene;
    public TextMeshProUGUI LevelStatus;
    bool LoadingNextScene;

    void Awake() {
        Instance = this;
    }

    void Update()
    {
        bool LevelIsCleared = Prize.AllInstancesCollected();

        LevelStatus.enabled = false;

        try {
            if (LevelStatus.enabled && !LoadingNextScene) {
                LoadingNextScene = true;
                Invoke("LoadNextScene", 4);
            }

        }
        catch (IndexOutOfRangeException) {

            Debug.Log("No Player Found.");

        }

    }

    void LateUpdate() {
        
    }
    
    void LoadNextScene() {
        if (NextScene == "") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        SceneManager.LoadScene(NextScene);
    }
}
