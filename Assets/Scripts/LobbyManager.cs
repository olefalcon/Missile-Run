using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    
    //Variable for syncing player names
    public List<string> playerNames;
    public int playerNum;
    //Player room object script (for ready changes)
    public RoomPlayer nrp;
    public NewNetworkRoomManager nm;
    //Booleans for players ready status
    private bool isP1Ready;
    private bool isP2Ready;
    private bool isP3Ready;
    private bool isP4Ready;
    //Materials for lighting up player icons
    public Material player1mat;
    public Material player2mat;
    public Material player3mat;
    public Material player4mat;
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
        nrp.readyUp();
    }
    public void leaveRoom() {
        NetworkManager.singleton.StopClient();
    }

    //Clients link to player command when they join room
    public void playerJoin(string name) {
        nrp = NetworkClient.connection.identity.GetComponent<RoomPlayer>();
        nrp.playerJoin(name);
    }
    //Return from room player command that rpcs on all clients
    public void playerJoinRpc(List<string> _playerNames) {
        playerNames = _playerNames;
        username1.text = playerNames[0];
        username2.text = playerNames[1];
        username3.text = playerNames[2];
        username4.text = playerNames[3];
    }
    //return from room player command that rpcs on all clients
    public void playerReadyRpc(int p) {
        switch(p)
        {
            case 1:
                isP1Ready = true;
                player1block.GetComponent<Renderer>().material = player1mat;
                break;
            case 2:
                isP2Ready = true;
                player2block.GetComponent<Renderer>().material = player2mat;
                break;
            case 3:
                isP3Ready = true;
                player3block.GetComponent<Renderer>().material = player3mat;
                break;
            case 4:
                isP4Ready = true;
                player4block.GetComponent<Renderer>().material = player4mat;
                break;
        }
    }
}
