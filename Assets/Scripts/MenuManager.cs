using Mirror;
using EpicTransport;
using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;
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
    public float preGameScreenTime;
    private float preGameScreenTimer;
    //Pre Game Screens
    public Image devAuthScreen;
    public Image developerScreen;
    public Image warningsScreen;
    //Menu buttons
    public Button hostGameButton;
    public Button joinGameButton;
    public Button settingsGameButton;
    public Button quitGameButton;
    //Menu panels
    public Image splashScreen;
    public Image hostMenu;
    public Image joinMenu;
    public Image settingsMenu;
    //Input fields
    public InputField hostIPField;
    public InputField hostNameField;
    public InputField joinIPField;
    public InputField joinNameField;
    public InputField devAuthField;
    //Network Manager
    public NewNetworkRoomManager nm;
    public EOSLobby eosLobby;

    void Start() {
        preGameScreenTimer = preGameScreenTime;
        nm = GameObject.Find("NetworkManager").GetComponent<NewNetworkRoomManager>();
        eosLobby = GameObject.Find("EOSManager").GetComponent<EOSLobby>();
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
    public void HostGame()
    {
        nm.playerName = hostNameField.text;
        NetworkManager.singleton.StartHost();
    }
    public void JoinGame()
    {
        nm.playerName = joinNameField.text;
        NetworkManager.singleton.networkAddress = joinIPField.text;
        NetworkManager.singleton.StartClient();
    }
    public void Settings() {
        settingsMenu.gameObject.SetActive(true);
    }
    public void AcceptSettings() {
        settingsMenu.gameObject.SetActive(false);
    }
    public void QuitGame() {
        Application.Quit();
    }
    public void HostMenu() {
        hostIPField.text = EOSSDKComponent.LocalUserProductIdString;
        hostMenu.gameObject.SetActive(true);
    }
    public void CopyId() {
        string id = hostIPField.text;
        id.CopyToClipboard();
    }
    public void HostMenuClose() {
        hostMenu.gameObject.SetActive(false);
    }
    public void JoinMenu() {
        joinMenu.gameObject.SetActive(true);
    }
    public void JoinMenuClose() {
        joinMenu.gameObject.SetActive(false);
    }
    public void InitEOS() {
        EOSSDKComponent eos = GameObject.Find("EOSManager").GetComponent<EOSSDKComponent>();
        eos.devAuthToolCredentialName = devAuthField.text;
        EOSSDKComponent.Initialize();
        devAuthScreen.gameObject.SetActive(false);
    }
    /*
    public void animateMenuButtons() {
        Sequence mainMenuSequence = DOTween.Sequence();
        mainMenuSequence.Append(hostGameButton.rectTransform.DOAnchorPosY(-40f,0.46875, false).SetEasy(Ease.OutQuint))
        .Append(joinGameButton.transform.DOMoveY(-60f,0.46875, false))
        .Append(settingsGameButton.transform.DOMoveY(-80f,0.46875, false))
        .Append(quitGameButton.transform.DOMoveY(-100f,0.46875, false));
        mainMenuSequence();
    }
    */
}