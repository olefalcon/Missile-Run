using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;
    [Header("Audio React Objects")]
    public GameObject score1;
    public GameObject score2;
    public GameObject score3;
    public GameObject score4;
    [Header("Audio Clips")]
    public AudioSource playerDeath;
    public AudioSource speed;
    public AudioSource ghost;
    public AudioSource shieldUp;
    public AudioSource shieldDown;
    public AudioSource playerSwap;
    public AudioSource glitch;
    public AudioSource roundEnd;
    public AudioSource missileHitWall;
    public AudioSource subDrop;
    public AudioSource music;
    public AudioClip[] drops;
    //Private tweens
    private Tween score1Tween;
    private Tween score2Tween;
    private Tween score3Tween;
    private Tween score4Tween;

    //Generic function for calling audio clips
    public void PlaySFX(string sfx) {
        //Converts the string sfx into the variable sfx and plays the clip
        AudioSource asource = (AudioSource)this.GetType().GetField(sfx).GetValue(this);
        asource.Play();
    }
    public void PlayMusic(int musicIndex) {
        music.clip = drops[musicIndex-1];
        music.Play();
        //audio react stuff
        /*
        score1Tween = score1.transform.DOScale(new Vector3(5.5f,1f,1.2f), 0.46875f).SetLoops(-1, LoopType.Restart).SetEase(Ease.OutQuint);
        score2Tween = score2.transform.DOScale(new Vector3(5.5f,1f,1.2f), 0.46875f).SetLoops(-1, LoopType.Restart).SetEase(Ease.OutQuint);
        score3Tween = score3.transform.DOScale(new Vector3(5.5f,1f,1.2f), 0.46875f).SetLoops(-1, LoopType.Restart).SetEase(Ease.OutQuint);
        score4Tween = score4.transform.DOScale(new Vector3(5.5f,1f,1.2f), 0.46875f).SetLoops(-1, LoopType.Restart).SetEase(Ease.OutQuint);
        score1Tween.Restart();
        score2Tween.Restart();
        score3Tween.Restart();
        score4Tween.Restart();
        */
    }
    //Function for lowpassing music at the end of round
    public void FilterMusic() {
        mixer.SetFloat("musicCutoff", 300f);
    }
    //Function for stopping music
    public void StopMusic() {
        mixer.SetFloat("musicCutoff", 22000f);
        music.Stop();
        //Stop audio reacts
        /*
        score1Tween.Pause();
        score2Tween.Pause();
        score3Tween.Pause();
        score4Tween.Pause();
        */
    }
}
