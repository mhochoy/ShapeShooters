using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffects : MonoBehaviour
{
    public int DestroyTime;
    [SerializeField] bool On;
    // Start is called before the first frame update
    void Start()
    {
        if (On) {
            Invoke("DestroyMe", DestroyTime);
        }
    }

    void DestroyMe() {
        Destroy(gameObject);
    }
}
