using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    [SerializeField]
    public static bool hasMenuOnce;
    // Start is called before the first frame update
    void Start()
    {
        hasMenuOnce = false;
        DontDestroyOnLoad(gameObject);
    }

}
