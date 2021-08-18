using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    //Menu settings
    public float pulseScale;
    //Menu timers
    public float beatTime;
    private float beatTimer;
    public float preGameScreenTime;
    private float preGameScreenTimer;
    //Pre Game Screens
    public Image developerScreen;
    public Image warningsScreen;
    //Menu buttons
    public Button hostGameButton;
    public Button joinGameButton;
    public Button settingsGameButton;
    public Button quitGameButton;

    void Start() {
        preGameScreenTimer = preGameScreenTime;
        beatTimer = 0;
    }

    void Update () {
        if (preGameScreenTimer <= 0) {
            if (developerScreen.gameObject.activeInHierarchy) {
                developerScreen.gameObject.SetActive(false);
            } else if (warningsScreen.gameObject.activeInHierarchy) {
                warningsScreen.gameObject.SetActive(false);
            } else {
                //animateMenuButtons();
            }
            preGameScreenTimer = preGameScreenTime;
        } else {
            preGameScreenTimer -= Time.deltaTime;
        }
    }
    /*
    public void HostGame()
    {
        NetworkManager.singleton.StartHost();
    }
    public void JoinGame()
    {
        NetworkManager.singleton.StartClient();
    }
    
    public void animateMenuButtons() {
        Sequence mainMenuSequence = DOTween.Sequence();
        mainMenuSequence.Append(hostGameButton.rectTransform.DOAnchorPosY(-40f,0.46875, false).SetEasy(Ease.OutQuint))
        .Append(joinGameButton.transform.DOMoveY(-60f,0.46875, false))
        .Append(settingsGameButton.transform.DOMoveY(-80f,0.46875, false))
        .Append(quitGameButton.transform.DOMoveY(-100f,0.46875, false));
        mainMenuSequence();
    */
}