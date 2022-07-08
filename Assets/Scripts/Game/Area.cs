using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public List<GameObject> enemies;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        int defeated_enemies = 0;
        foreach (GameObject enemy in enemies) {
            if (!enemy.activeInHierarchy) defeated_enemies++;
        }

        if (defeated_enemies == enemies.Count) {
            if (!animator.GetBool("open")) {
                animator.SetBool("open", true);
                animator.Play("DoorOpen");
            }
        }
    }
}
