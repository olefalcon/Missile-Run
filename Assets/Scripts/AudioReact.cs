using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioReact : MonoBehaviour
{
    //whether or not component will react when it awakes
    public bool playOnAwake;

    //Effects to react to
    public bool reactGrow;
    public bool reactRotate;
    public bool reactScore;

    //Number of reaction loops (-1 for inf)
    public int reactionLoops;

    // Start is called before the first frame update
    void Start()
    {
        if (playOnAwake) {
            StartReact();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartReact() {
        if (reactGrow) {
            transform.DOScale(1.05f, 0.46875f).SetLoops(reactionLoops, LoopType.Restart).SetEase(Ease.OutQuint);
        }
        if (reactScore) {
            //transform.DOLocalMoveX(Random.Range(-9.5f, -8.5f), 0.46875f).SetLoops(reactionLoops, LoopType.Restart).SetEase(Ease.OutQuint);
        }
    }


}
