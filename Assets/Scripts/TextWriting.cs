using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextWriting : MonoBehaviour
{
    void Start() {
        Text text = GetComponent<Text>();
        text.DOText("Waiting for players to join...", 0.46875f*2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
    
}
