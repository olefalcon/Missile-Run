using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LobbyManager : MonoBehaviour
{
    
    //Variable for syncing player names
    public List<string> playerNames;
    public int playerNum;
    //Player room object script (for ready changes)
    public RoomPlayer nrp;
    public NewNetworkRoomManager nm;
    //Materials for lighting up player icons
    public Material player1mat;
    public Material player2mat;
    public Material player3mat;
    public Material player4mat;
    //Images for ready up
    public RawImage player1Ready;
    public RawImage player2Ready;
    public RawImage player3Ready;
    public RawImage player4Ready;
    //Materials for dull non readied players
    public Material player1dullMat;
    public Material player2dullMat;
    public Material player3dullMat;
    public Material player4dullMat;
    //Blocks representing each player
    public GameObject player1block;
    public GameObject player2block;
    public GameObject player3block;
    public GameObject player4block;
    //Text for players names
    public Text username1;
    public Text username2;
    public Text username3;
    public Text username4;
    //buttons for ready up
    public Button readyButton;
    public Button unReadyButton;
    public Button quitButton;
    public Text allReadyText;
    public Text statusText;
    // Start is called before the first frame update
    void Start()
    {
        playerNames = new List<string>();
        playerNum = 0;
        username1.text = "";
        username2.text = "";
        username3.text = "";
        username4.text = "";
    }

    public void readyUp() {
        nrp = NetworkClient.connection.identity.GetComponent<RoomPlayer>();
        readyButton.gameObject.SetActive(false);
        unReadyButton.gameObject.SetActive(true);
        nrp.readyUp();
    }
    public void unReadyUp() {
        nrp = NetworkClient.connection.identity.GetComponent<RoomPlayer>();
        unReadyButton.gameObject.SetActive(false);
        readyButton.gameObject.SetActive(true);
        nrp.unReadyUp();
    }
    public void leaveRoom() {
        NetworkManager.singleton.StopHost();
        NetworkManager.singleton.StopClient();
    }

    //Clients link to player command when they join room
    public void playerJoin(string name) {
        nrp = NetworkClient.connection.identity.GetComponent<RoomPlayer>();
        nrp.playerJoin(name);
    }
    //Return from room player command that rpcs on all clients
    public void playerJoinRpc(List<string> _playerNames) {
        updateLobbyNames(_playerNames);
    }
    //return from room player command that rpcs on all clients
    public void playerReadyRpc(int p) {
        Debug.Log("Player Ready Rpc p=" + p);
        switch(p)
        {
            case 0:
                player1Ready.gameObject.SetActive(true);
                break;
            case 1:
                player2Ready.gameObject.SetActive(true);
                break;
            case 2:
                player3Ready.gameObject.SetActive(true);
                break;
            case 3:
                player4Ready.gameObject.SetActive(true);
                break;
        }
    }
    public void playerUnReadyRpc(int p) {
        switch(p)
        {
            case 0:
                player1Ready.gameObject.SetActive(false);
                break;
            case 1:
                player2Ready.gameObject.SetActive(false);
                break;
            case 2:
                player3Ready.gameObject.SetActive(false);
                break;
            case 3:
                player4Ready.gameObject.SetActive(false);
                break;
        }
    }
    public void allPlayerReady() {
        unReadyButton.gameObject.SetActive(false);
        readyButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        allReadyText.gameObject.SetActive(true);
        allReadyText.DOText("Game Starting...", 0.46875f*2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }


    //Feedback from server for client leaving
    public void playerLeaveRpc(int p, string name, List<string> playerNames) {
        updateLobbyNames(playerNames);
        string sText = name + " has left the lobby!";
        statusText.DOText(sText, 0.46875f*4f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    //Update player names from lobby
    public void updateLobbyNames(List<string> _playerNames) {
        playerNames = _playerNames;
        username1.text = playerNames[0];
        username2.text = playerNames[1];
        username3.text = playerNames[2];
        username4.text = playerNames[3];
    }

}
