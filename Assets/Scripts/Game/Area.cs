using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Area : MonoBehaviour
{
    public List<GameObject> enemies;
    public bool cleared {get; private set;}
    public GameObject Door;
    [SerializeField] Camera AreaCamera;
    [SerializeField] List<GameObject> SpawnOnAreaEnter;
    [SerializeField] List<GameObject> HideOnAreaLeave;
    Animator _DoorAnimator;
    AudioSource _Audio;

    void Awake()
    {
        _Audio = GetComponent<AudioSource>();
        if (Door) {
            _DoorAnimator = Door.GetComponentInChildren<Animator>();
        }
        AreaCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.All((enemy) => enemy.activeSelf == false)) {
            OpenAreaDoor();
            cleared = true;
        }
    }

    public void Activate() {
        AreaCamera.enabled = true;
        if (SpawnOnAreaEnter.Count > 0) {
            foreach (GameObject ob in SpawnOnAreaEnter) {
                ob.SetActive(true);
            }
        }
    }

    public void Deactivate() {
        AreaCamera.enabled = false;
        if (HideOnAreaLeave.Count > 0) {
            foreach (GameObject ob in HideOnAreaLeave) {
                ob.SetActive(false);
            }
        }
    }

    void OpenAreaDoor() {
        if (Door && !_DoorAnimator.GetBool("open")) {
            _DoorAnimator.SetBool("open", true);
            _DoorAnimator.Play("DoorOpen");
            _Audio.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        foreach (Player player in Player.allPlayers) {
            if (player.gameObject == other.gameObject) {
                AreaCamera.enabled = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        foreach (Player player in Player.allPlayers) {
            if (player.gameObject == other.gameObject) {
                AreaCamera.enabled = false;
            }
        }
    }
}
