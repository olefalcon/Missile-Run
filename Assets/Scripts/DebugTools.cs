using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : MonoBehaviour
{
    private GameManager gms;
    public string endRoundKey;

    void Start() {
        gms = gameObject.GetComponent<GameManager>();
    }

    void Update() {

        if (Input.GetKeyDown(endRoundKey)) {
            gms.EndRound(0);
        }

    }
}
