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
    public CinemachineTargetGroup cameraTargets;
    public TextMeshProUGUI LevelStatus;
    List<GameObject> _PlayerHistory = new List<GameObject>();
    List<GameObject> _EnemyHistory = new List<GameObject>();
    bool LoadingNextScene;

    void Update()
    {
        bool PrizesAreCollected = prizes.TrueForAll( ( Prize prize ) => { return prize.collected; } );
        bool AreasIsCleared = areas.TrueForAll( ( Area area ) => { return area.cleared; } );
        bool LevelIsCleared = PrizesAreCollected && AreasIsCleared;

        try {
            
            //camera.m_Follow = MainPlayer.transform;
            if (HandleLevelStatus(LevelIsCleared) && !LoadingNextScene) {
                LoadingNextScene = true;
                Invoke("LoadNextScene", 4);
            }

        }
        catch (IndexOutOfRangeException) {

            Debug.Log("No Player Found.");

        }

    }

    void LateUpdate() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        // Add All current players to be tracked by the camera
        if (players.Length > 0) {
            foreach (GameObject player in players) {
                if (!_PlayerHistory.Contains(player) && player.layer != 2) {
                    cameraTargets.AddMember(player.transform, 1, player.transform.localScale.x);
                    _PlayerHistory.Add(player);
                }
            }
        }
        // Keep the weight of all alive players updated
        if (_PlayerHistory.Count > 0) {
            foreach (GameObject player in _PlayerHistory) {
                Player _player;

                player.TryGetComponent<Player>(out _player);
                if (_player && _PlayerHistory.Contains(player)) {
                    if (!_player.enabled || !player.activeInHierarchy) {
                        cameraTargets.m_Targets[cameraTargets.FindMember(player.transform)].weight = 0;
                    }
                    if (_player.enabled) {
                        cameraTargets.m_Targets[cameraTargets.FindMember(player.transform)].weight = 1;
                    }
                }
            }
        }
        

        // Add all enemies to be tracked by the camera and keep weight updated 
        foreach (Area area in areas) {
            foreach (GameObject enemy in area.enemies) {
                if (!_EnemyHistory.Contains(enemy)) {
                    cameraTargets.AddMember(enemy.transform, .3f, 3f);
                    _EnemyHistory.Add(enemy);
                }
                Player _enemy;
            
                enemy.TryGetComponent<Player>(out _enemy);

                if (_enemy && _EnemyHistory.Contains(enemy)) {
                    if (!_enemy.IsLockedOntoTarget() || _enemy.enabled == false) {
                        cameraTargets.m_Targets[cameraTargets.FindMember(enemy.transform)].weight = 0;
                    }
                    else if (_enemy.IsLockedOntoTarget()) {
                        cameraTargets.m_Targets[cameraTargets.FindMember(enemy.transform)].weight = .3f;
                    }
                }    
            }
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
