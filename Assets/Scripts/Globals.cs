using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    [SerializeField] public static bool hasMenuOnce;
    //Game settings
    [SerializeField] public static float powerupSpawnTime;
    [SerializeField] public static int pillars;
    [SerializeField] public static float missileStartSpeed;
    [SerializeField] public static float missileSpeedGain;
    [SerializeField] public static float missileStartTurnRate;
    [SerializeField] public static float missileTurnRateGain;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
