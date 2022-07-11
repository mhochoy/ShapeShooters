using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour
{
    public GameObject PrizeObject;
    public bool collected {get; private set;}

    public void CollectPrize() {
        collected = true;
        PrizeObject.SetActive(false);
    }
}
