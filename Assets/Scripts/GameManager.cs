using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    //Audio Manager
    public AudioManager am;
    //Object Data
    public GameObject playerAIPrefab;
    public GameObject missilePrefab;
    public GameObject powerupParent;
    public GameObject powerupPrefab;
    public GameObject pillarsParent;
    public GameObject pillarPrefab;
    //Network Manager
    public NewNetworkRoomManager nm;
    //Cam Data and Positions
    public Camera cam;
    public Vector3 lobbyCameraPos;
    public Vector3 lobbyCameraRot;
    public Vector3 gameCameraPos;
    public Vector3 gameCameraRot;
    public Vector3 endCameraPos;
    public Vector3 endCameraRot;
    public float camZoomScaling;
    public float timeSlowScaling;
    public float camMaxZoom;
    public float camNormZoom;
    //Game State
    [SyncVar]
    public bool isStart;
    public float startDelay;
    public float startDelayTimer;
    public float numHumanPlayers;
    public Image roundWinnerBanner;
    public float bannerScrollSpeed;
    public Text roundWinnerText;
    public Transform spawn1;
    public Transform spawn2;
    public Transform spawn3;
    public Transform spawn4;
    public Material player1mat;
    public Material player2mat;
    public Material player3mat;
    public Material player4mat;
    public Transform missileSpawn;
    public float numPillars;
    public float interferenceRange;
    public float powerupSpawnInterval;
    private float powerupSpawnTimer;
    private float endRoundTimer;
    public float endRoundTime;
    public bool isEndRound = false;
    //Score Data
    public int player1Score;
    public Text player1ScoreText;
    public int player2Score;
    public Text player2ScoreText;
    public int player3Score;
    public Text player3ScoreText;
    public int player4Score;
    public Text player4ScoreText;
    //Overlays
    public Image loadingScreen;
    //Player Data
    public GameObject[] players;
    public Transform[] playerSpawns;
    public Material[] playerMaterials;
    public Material[] powerupMaterials;
    public GameObject missile;
    // Start is called before the first frame update
    void Start()
    {
        nm = GameObject.Find("NetworkManager").GetComponent<NewNetworkRoomManager>();
        InitScores();
        SetupArrays();
        CompilePlayerSpawns();
        CompilePlayerMaterials();
        startDelayTimer = startDelay;
        //We will start the round when all clients are loaded in
        //StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        //No actions need to be done on update on a client
        if (!isServer) {return;}
        //Check for new powerup spawn
        if (!isStart) {
            if (startDelayTimer <= 0) {
                isStart = true;
                StartRound();
            } else {
                startDelayTimer -= Time.deltaTime;
                return;
            }
        }
        
        if (powerupSpawnTimer <= 0)
        {
            SpawnPowerup();
            powerupSpawnTimer = powerupSpawnInterval;
        }
        powerupSpawnTimer -= Time.deltaTime;
        //Check if end round timer is up
        if (isEndRound)
        {
            if (endRoundTimer <= 0)
            {
                RestartRound();
            }
            endRoundTimer -= Time.deltaTime;
            /*
            if (cam.orthographicSize >= camMaxZoom)
            {
                cam.orthographicSize -= Time.deltaTime * camZoomScaling;
            }
            */
            Time.timeScale -= Time.deltaTime * timeSlowScaling;
        }
    }
    //Function to leave game
    public void leaveGame() {
        if (isServer) {NetworkManager.singleton.StopHost();}
        else {NetworkManager.singleton.StopClient();}
    }
    //Function to start a round
    [ClientRpc]
    public void StartRound()
    {
        am.PlaySFX("subDrop");
        if (loadingScreen.gameObject.activeInHierarchy) {
            loadingScreen.gameObject.SetActive(false);
        }
        if (!isServer) {return;}
        CreateMissile();
        int numPlayers = nm.numPlayers;
        for (int i=numPlayers;i<4;i++) {
            CreatePlayerAI(i);
        }
        //CreatePlayer(0);
        //CreatePlayer(1);
        //CreatePlayer(2);
        //CreatePlayer(3);
        SpawnPillars();
        SpawnPowerup();
        powerupSpawnTimer = powerupSpawnInterval;
    }
    //Function when round ends
    [ClientRpc]
    public void EndRound(int winnerIndex)
    {
        isEndRound = true;
        endRoundTimer = endRoundTime;
        //End of round cam
        //cam.transform.position = new Vector3(missile.transform.position.x, 10f, missile.transform.position.z);
        //cam.orthographicSize = camMaxZoom;
        string winnerColor = indexToColor(winnerIndex);
        roundWinnerText.text = winnerColor + " Wins!";
        roundWinnerBanner.transform.localPosition = new Vector3(181f, 0f, 0f);
        //Score Handling
        Text winnerScoreText = player1ScoreText;
        int winnerScore = 0;
        switch (winnerIndex) {
            case 0:
                ++player1Score;
                winnerScoreText = player1ScoreText;
                winnerScore = player1Score;
                break;
            case 1:
                ++player2Score;
                winnerScoreText = player2ScoreText;
                winnerScore = player2Score;
                break;
            case 2:
                ++player3Score;
                winnerScoreText = player3ScoreText;
                winnerScore = player3Score;
                break;
            case 3:
                ++player4Score;
                winnerScoreText = player4ScoreText;
                winnerScore = player4Score;
                break;
        }
        winnerScoreText.text = winnerScore.ToString();
        //EndRound sound
        am.StopMusic();
        am.PlaySFX("roundEnd");
    }
    //Function when round is restarting
    [ClientRpc]
    public void RestartRound()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (isServer) {
                Debug.Log(player.name);
            }
            if (player.GetComponent<PlayerAI>() != null && isServer) {
                Destroy(player);
            } else {
                //Move player on local client because of client authority movement
                if (player.GetComponent<NetworkIdentity>().isLocalPlayer) {
                    player.transform.position = playerSpawns[player.GetComponent<Player>().pIndex].position;
                }
                if (isServer) {
                    player.GetComponent<Player>().isAlive = true;
                }
                player.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        if (isServer) {
            
            Destroy(missile);
            foreach (Transform pillar in pillarsParent.transform)
            {
                Destroy(pillar.gameObject);
            }
            foreach (Transform powerup in powerupParent.transform)
            {
                Destroy(powerup.gameObject);
            }
        }
        
        Time.timeScale = 1f;
        isEndRound = false;
        //cam.orthographicSize = camNormZoom;
        //cam.transform.position = defaultCameraPos;
        roundWinnerBanner.transform.localPosition = new Vector3(-1500f, 100f, 0f);
        if (isServer) {
            StartRound();
        }
    }
    //Function to create player
    public void CreatePlayerAI(int index)
    {
        GameObject ai = (Instantiate(playerAIPrefab, playerSpawns[index].position, Quaternion.identity));
        ai.GetComponent<Player>().material = playerMaterials[index];
        ai.GetComponent<Player>().pIndex = index;
        NetworkServer.Spawn(ai);
    }
    //Function to create a missile
    public void CreateMissile()
    {
        missile = Instantiate(missilePrefab, missileSpawn.position, Quaternion.identity);
        missile.name = "Missile";
        NetworkServer.Spawn(missile.gameObject, connectionToServer);
    }

    //Function to determine array lengths
    public void SetupArrays()
    {
        players = new GameObject[4];
        playerSpawns = new Transform[4];
        playerMaterials = new Material[4];
    }
    //Function to compile player spawns
    public void CompilePlayerSpawns()
    {
        playerSpawns[0] = spawn1;
        playerSpawns[1] = spawn2;
        playerSpawns[2] = spawn3;
        playerSpawns[3] = spawn4;
    }
    //Function to compile player materials
    public void CompilePlayerMaterials()
    {
        playerMaterials[0] = player1mat;
        playerMaterials[1] = player2mat;
        playerMaterials[2] = player3mat;
        playerMaterials[3] = player4mat;
    }
    //Function to spawn pillars
    public void SpawnPillars()
    {
        for (int i = 0; i < numPillars; i++)
        {
            Vector3 pillarPos = Vector3.zero;
            bool pillarPosCheck = false;
            while (pillarPosCheck == false)
            {
                pillarPos = new Vector3(Random.Range(-4f, 4f), 1f, Random.Range(-4f, 4f));
                if (CheckPillarInterferences(pillarPos) == true) { pillarPosCheck = true; }
            }
            GameObject pillar = Instantiate(pillarPrefab, pillarPos, Quaternion.identity, pillarsParent.transform);
            NetworkServer.Spawn(pillar.gameObject);
        }
    }
    //Function used to check if a chosen pillar spawn location conflicts with a player or missile's start location
    public bool CheckPillarInterferences(Vector3 pos)
    {
        if (Vector3.Distance(pos, spawn1.position) < interferenceRange) { return false; }
        if (Vector3.Distance(pos, spawn2.position) < interferenceRange) { return false; }
        if (Vector3.Distance(pos, spawn3.position) < interferenceRange) { return false; }
        if (Vector3.Distance(pos, spawn4.position) < interferenceRange) { return false; }
        if (Vector3.Distance(pos, missileSpawn.position) < interferenceRange) { return false; }
        return true;
    }
    //function to spawn powerup
    public void SpawnPowerup()
    {
        Vector3 powerupPos = new Vector3(Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
        GameObject powerup = Instantiate(powerupPrefab, powerupPos, Quaternion.identity, powerupParent.transform);
        NetworkServer.Spawn(powerup.gameObject);
    }
    //function to init all scores and score texts to 0
    public void InitScores()
    {
        player1Score = 0;
        player1ScoreText.text = "0";
        player2Score = 0;
        player2ScoreText.text = "0";
        player3Score = 0;
        player3ScoreText.text = "0";
        player4Score = 0;
        player4ScoreText.text = "0";
    }
    //function that returns the color associated with a player index --used to announce what color wins
    public string indexToColor(int index) {
        switch (index) {
            case 0:
                return "Blue";
            case 1:
                return "Red";
            case 2:
                return "Yellow";
            case 3:
                return "Green";
        }
        return "Nobody";
    }
}
