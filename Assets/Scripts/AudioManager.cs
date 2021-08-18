using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Generic function for calling audio clips
    public void PlaySFX(string sfx) {
        //Converts the string sfx into the variable sfx and plays the clip
        AudioSource asource = (AudioSource)this.GetType().GetField(sfx).GetValue(this);
        asource.Play();
    }
    //Function for stopping music
    public void StopMusic() {
        music.Stop();
    }
}
