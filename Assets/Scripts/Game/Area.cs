using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public List<GameObject> enemies;
    public bool cleared {get; private set;}
    public GameObject Door;
    Animator _DoorAnimator;
    List<GameObject> defeated_enemies = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        _DoorAnimator = Door.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject enemy in enemies) {
            if (!enemy.activeInHierarchy && !defeated_enemies.Contains(enemy)) defeated_enemies.Add(enemy);
        }

        if (defeated_enemies.Count > 0 && (defeated_enemies.Count == enemies.Count)) {
            OpenAreaDoor();
        }
    }

    void OpenAreaDoor() {
        if (!_DoorAnimator.GetBool("open")) {
            _DoorAnimator.SetBool("open", true);
            _DoorAnimator.Play("DoorOpen");
            cleared = true;
        }
    }
}
