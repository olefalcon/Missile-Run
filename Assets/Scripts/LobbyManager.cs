using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    //Variable for syncing player names
    [SyncVar]
    public List<string> playerNames;
    public int playerNum;
    //Player room object script (for ready changes)
    public NetworkRoomPlayer nrp;
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
        nm = GameObject.Find("NetworkManager").GetComponent<NewNetworkRoomManager>();
        nrp = nm.roomSlots[playerNum];
    }

    public void readyUp() {
        nrp.CmdChangeReadyState(true);
    }
    public void leaveRoom() {
        NetworkManager.singleton.StopClient();
    }

    //Clients command when they join room
    [Command]
    public void playerJoin(string name) {
        playerNames.Add(name);
        playerJoinRpc();
    }
    [ClientRpc]
    //Call from network manager when player enters room
    public void playerJoinRpc() {        
        username1.text = playerNames[0];
        username2.text = playerNames[1];
        username3.text = playerNames[2];
        username4.text = playerNames[3];
    }
    //Command to server when player readys up
    [Command]
    public void playerReady(int p) {
        playerReadyRpc(p);
    }
    [ClientRpc]
    //Call from network manager when player readys up
    public void playerReadyRpc(int p)
    {
        switch(p)
        {
            case 0:
                isP1Ready = true;
                player1block.GetComponent<Renderer>().material = player1mat;
                break;
            case 1:
                isP2Ready = true;
                player2block.GetComponent<Renderer>().material = player2mat;
                break;
            case 2:
                isP3Ready = true;
                player3block.GetComponent<Renderer>().material = player3mat;
                break;
            case 3:
                isP4Ready = true;
                player4block.GetComponent<Renderer>().material = player4mat;
                break;
        }
    }
    [ClientRpc]
    //Call from network manager when player unreadys
    public void RpcPlayerUnready(int p)
    {
        switch(p)
        {
            case 0:
                isP1Ready = false;
                player1block.GetComponent<Renderer>().material = player1dullMat;
                break;
            case 1:
                isP2Ready = false;
                player2block.GetComponent<Renderer>().material = player2dullMat;
                break;
            case 2:
                isP3Ready = false;
                player3block.GetComponent<Renderer>().material = player3dullMat;
                break;
            case 3:
                isP4Ready = false;
                player4block.GetComponent<Renderer>().material = player4dullMat;
                break;
        }
    }
}
