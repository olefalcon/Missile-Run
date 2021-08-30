using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class Player : NetworkBehaviour
{
    public float baseSpeed;
    public bool hasPowerupEffect;
    public int powerupType;
    public float powerupTimer;
    [SyncVar]
    public bool isAlive;
    public bool isInvis;
    public Vector3 glitchPosition;
    public int glitchMarksToPlace;
    public GameObject glitchMarkPrefab;
    public GameObject glitchModule;
    public bool hasShield;
    public Material material;
    public ParticleSystem ps;
    public ParticleSystemRenderer psr;
    [SyncVar]
    public int pIndex;
    //Private 
    public float speed;
    //Materials for assignment
    public Material player1mat;
    public Material player2mat;
    public Material player3mat;
    public Material player4mat;

    public Vector3 direction;
    public Vector3 spawnLocation;

    void Start()
    {
        if (isLocalPlayer) {
            NewNetworkRoomManager nm = GameObject.Find("NetworkManager").GetComponent<NewNetworkRoomManager>();
            DeterminePlayerIndex(nm.playerNum);
        } else {
            AssignMat(pIndex);
        }
        spawnLocation = transform.position;
        direction = new Vector3(0f,0f,0f);
        speed = baseSpeed;
        hasPowerupEffect = false;
        isAlive = true;
    }

    void Update()
    {
        if (hasPowerupEffect)
        {
            if (powerupType == 4)
            {
                if (powerupTimer <= 2.5 && glitchMarksToPlace >= 5)
                {
                    Instantiate(glitchMarkPrefab, transform.position, Quaternion.identity, glitchModule.transform);
                    glitchMarksToPlace--;
                }
                if (powerupTimer <= 2 && glitchMarksToPlace >= 4)
                {
                    Instantiate(glitchMarkPrefab, transform.position, Quaternion.identity, glitchModule.transform);
                    glitchMarksToPlace--;
                }
                if (powerupTimer <= 1.5 && glitchMarksToPlace >= 3)
                {
                    Instantiate(glitchMarkPrefab, transform.position, Quaternion.identity, glitchModule.transform);
                    glitchMarksToPlace--;
                }
                if (powerupTimer <= 1 && glitchMarksToPlace >= 2)
                {
                    Instantiate(glitchMarkPrefab, transform.position, Quaternion.identity, glitchModule.transform);
                    glitchMarksToPlace--;
                }
                if (powerupTimer <= 0.5 && glitchMarksToPlace >= 1)
                {
                    Instantiate(glitchMarkPrefab, transform.position, Quaternion.identity, glitchModule.transform);
                    glitchMarksToPlace--;
                }


            }
            if (powerupTimer <= 0)
            {
                //Remove powerup effect
                if (powerupType == 1)
                {
                    speed = baseSpeed;
                    transform.GetChild(4).gameObject.SetActive(false);
                    transform.GetChild(2).gameObject.SetActive(true);
                } else if (powerupType == 3)
                {
                    isInvis = false;
                    gameObject.GetComponentInChildren<Renderer>().material = material;
                } else if (powerupType == 4)
                {
                    transform.position = glitchPosition;
                    Destroy(glitchModule);
                } else if (powerupType == 5)
                {
                    hasShield = false;
                    transform.GetChild(3).gameObject.SetActive(false);
                }
                hasPowerupEffect = false;
            } else
            {
                powerupTimer -= Time.deltaTime;
            }
        }
    }

    void FixedUpdate()
    {
        if (direction.magnitude >= 0.1f && isAlive)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
            //Bounds checks
            if (transform.position.x < -4.5f) {
                transform.position = new Vector3(-4.5f, transform.position.y, transform.position.z);
            } else if (transform.position.x > 4.5f) {
                transform.position = new Vector3(4.5f, transform.position.y, transform.position.z);
            }
            if (transform.position.z < -4.5f) {
                transform.position = new Vector3(transform.position.x, transform.position.y, -4.5f);
            } else if (transform.position.z > 4.5f) {
                transform.position = new Vector3(transform.position.x, transform.position.y, 4.5f);
            }
        }
    }

    [ClientRpc]
    public void Die()
    {
        ps.Play();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        isInvis = false;
        gameObject.GetComponentInChildren<Renderer>().material = material;
        //is alive has to be set on the missile script because of delay
        //isAlive = false;
    }
    [Command]
    void DeterminePlayerIndex(int playerNum) {
        pIndex = playerNum;
        RpcAssignMat(playerNum);
    }
    [ClientRpc]
    void RpcAssignMat(int playerNum) {
        AssignMat(playerNum);
    }
    void AssignMat(int playerNum)
    {
        //Find material from player index
        switch(playerNum) {
            case 0:
                material = player1mat;
                break;
            case 1:
                material = player2mat;
                break;
            case 2:
                material = player3mat;
                break;
            case 3:
                material = player4mat;
                break;
        }
        //Apply mat to gameobject
        gameObject.GetComponentInChildren<Renderer>().material = material;
        ps = gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        psr = gameObject.transform.GetChild(1).gameObject.GetComponentInChildren<ParticleSystemRenderer>();
        psr.material = material;
        psr.trailMaterial = material;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (!isLocalPlayer && collider.GetComponent<PlayerAI>() == false) {return;}
        if (!isServer && collider.GetComponent<PlayerAI>() == true) {return;}
        if (collider.gameObject.tag == "Powerup")
        {
            if (hasPowerupEffect == false)
            {
                CmdActivatePowerup(collider.gameObject);
            }
        }
    }
    [Command]
    void CmdActivatePowerup(GameObject collider) {
        collider.GetComponent<Powerup>().ActivatePowerup(gameObject);
    }
}
