using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BgScroll : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveZ(15f, speed).SetLoops(-1,LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
