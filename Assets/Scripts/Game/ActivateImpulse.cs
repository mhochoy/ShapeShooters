using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ActivateImpulse : MonoBehaviour
{
    public float ImpulseStrength;
    CinemachineImpulseSource Impulse;
    void Start()
    {
        TryGetComponent<CinemachineImpulseSource>(out Impulse);
        if (Impulse) {
            Impulse.GenerateImpulse(transform.up * ImpulseStrength);
        }
    }
}
