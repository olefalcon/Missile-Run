using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public AudioManager am;
    public float lifespan;
    public float rotateSpeed;
    public float speedBonus;
    public float speedTime;
    public float invisTime;
    public Material invisPlayerMat;
    public float glitchDelay;
    public GameObject glitchModulePrefab;
    public float shieldTime;
    private float lifespanTimer;
    public int powerupType;
    public Material speedMat;
    public Material invisMat;
    public Material swapMat;
    public Material glitchMat;
    public Material shieldMat;
    public Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        lifespanTimer = lifespan;
        AssignType();
        AssignMat();
    }

    // Update is called once per frame
    void Update()
    {
        if (lifespanTimer <= 0)
        {
            Destroy(gameObject);
        }
        transform.Rotate(new Vector3(1f, 1f, 1f)* Time.deltaTime * rotateSpeed, Space.World);
    }

    public void AssignType()
    {
        powerupType = Mathf.RoundToInt(Random.Range(1f, 5f));
    }

    public void AssignMat()
    {
        rend = gameObject.GetComponent<Renderer>();
        if (powerupType == 1)
        {
            rend.material = speedMat;
        } else if (powerupType == 2)
        {
            rend.material = swapMat;
        } else if (powerupType == 3)
        {
            rend.material = invisMat;
        } else if (powerupType == 4)
        {
            rend.material = glitchMat;
        } else if (powerupType == 5)
        {
            rend.material = shieldMat;
        }
    }

    public void ActivatePowerup(GameObject player)
    {
        switch(powerupType)
        {
            case 1: //Powerup 1 = Speed Burst
                player.GetComponent<Player>().speed = speedBonus;
                player.GetComponent<Player>().transform.GetChild(4).gameObject.SetActive(true);
                player.GetComponent<Player>().transform.GetChild(2).gameObject.SetActive(false);
                player.GetComponent<Player>().powerupTimer = speedTime;
                player.GetComponent<Player>().hasPowerupEffect = true;
                player.GetComponent<Player>().powerupType = powerupType;
                am.PlaySFX("speed");
                break;
            case 2: //Powerup 2 = Player Swap
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                float farthestDist = 0f;
                GameObject farthestPlayer = player;
                foreach (GameObject p in players) {
                    float distToPlayer = Vector3.Distance(p.transform.position, player.transform.position);
                    if (p != player && distToPlayer >= farthestDist && p.GetComponent<Player>().isAlive)
                    {
                        farthestPlayer = p;
                        farthestDist = distToPlayer;
                    }
                }
                if (farthestPlayer != player)
                {
                    Vector3 farthestPlayerPos = farthestPlayer.transform.position;
                    Vector3 currentPlayerPos = player.transform.position;
                    farthestPlayer.transform.position = currentPlayerPos;
                    player.transform.position = farthestPlayerPos;
                    am.PlaySFX("playerSwap");
                }
                break;
            case 3: //Powerup 3 = Invisible
                player.GetComponent<Player>().isInvis = true;
                player.GetComponent<Player>().powerupTimer = invisTime;
                player.GetComponent<Player>().hasPowerupEffect = true;
                player.GetComponent<Player>().powerupType = powerupType;
                player.GetComponentInChildren<Renderer>().material = invisPlayerMat;
                am.PlaySFX("ghost");
                break;
            case 4: //Powerup 4 = Time Glitch
                player.GetComponent<Player>().glitchPosition = player.transform.position;
                player.GetComponent<Player>().powerupTimer = glitchDelay;
                player.GetComponent<Player>().hasPowerupEffect = true;
                player.GetComponent<Player>().powerupType = powerupType;
                player.GetComponent<Player>().glitchMarksToPlace = 5;
                GameObject glitchModule = Instantiate(glitchModulePrefab, player.transform.position, Quaternion.identity);
                player.GetComponent<Player>().glitchModule = glitchModule;
                am.PlaySFX("glitch");
                break;
            case 5: //Powerup 5 = Shield
                player.GetComponent<Player>().hasShield = true;
                player.GetComponent<Player>().powerupTimer = shieldTime;
                player.GetComponent<Player>().hasPowerupEffect = true;
                player.GetComponent<Player>().powerupType = powerupType;
                player.transform.GetChild(3).gameObject.SetActive(true);
                am.PlaySFX("shieldUp");
                break;

        }
        Destroy(gameObject);
    }
}
