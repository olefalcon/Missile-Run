using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : NetworkBehaviour
{
    public void HostGame()
    {
        NetworkManager.singleton.StartHost();
    }
    public void JoinGame()
    {
        NetworkManager.singleton.StartClient();
    }
}