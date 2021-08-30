using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : NetworkBehaviour
{
    public AudioManager am;
    public float startDelay;
    private float startDelayTimer;
    public GameObject gameManager;
    public Material missileMat;
    public float speed;
    public float acceleration;
    public float speedScaling;
    public float accelerationScaling;
    public GameObject[] players;
    [SyncVar]
    public GameObject target;
    //State 0 equals not chasing
    //state 1 equals chasing
    //state 2 equals starting delay
    //state 3 equals no targets roam
    [SyncVar]
    public int state;

    public float confusionTime;
    private float confusionTimer;

    //Private variables
    private Vector3 oldDirection;
    private Vector3 direction;
    private float closestDist;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        GameManager gameManagerScript = gameManager.GetComponent<GameManager>();
        direction = Vector3.zero;
        state = 2;
        startDelayTimer = startDelay;
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 2)
        {
            if (startDelayTimer <= 0)
            {
                int startTarget = Random.Range(0,players.Length);
                target = players[startTarget];
                state = 1;
                am.PlaySFX("music");
            } else
            {
                startDelayTimer -= Time.deltaTime;
            }
        }
        else if (!isServer) {
            //Clients only need to change the material of the rocket
            if (state == 2) {
                gameObject.GetComponentInChildren<Renderer>().material = missileMat;
            } else{
                gameObject.GetComponentInChildren<Renderer>().material = target.GetComponentInChildren<Renderer>().material;
            }
            return;
        }
        else if (state == 1)
        {
            closestDist = 100;
            //If current target is alive set closest dist to its position
            if (target.GetComponent<Player>().isAlive == true)
            {
                closestDist = Vector3.Distance(target.transform.position, transform.position);
            }
            foreach (GameObject player in players)
            {
                //Player will be null for 1 update cycle when ai spawns between rounds
                //This check prevents a non game breaking missingReferenceException
                if (player == null) {return;}
                float newDist = Vector3.Distance(player.transform.position, transform.position);
                //Compare each player that is alive and not invisible's distance to the current target
                if (player.GetComponent<Player>().isAlive == true && newDist < closestDist && player.GetComponent<Player>().isInvis == false)
                {
                    target = player;
                    closestDist = newDist;
                }
            }
            //If all available targets are invis when missile kills player
            if (target.GetComponent<Player>().isAlive == false || target.GetComponentInChildren<Player>().isInvis == true)
            {
                gameObject.GetComponentInChildren<Renderer>().material = missileMat;
            }
            else
            {
                ChangeTarget(target);
                float lerpRatio = acceleration;
                oldDirection = direction;
                direction = (target.transform.position - transform.position).normalized;
                direction = Vector3.Lerp(oldDirection, direction, lerpRatio).normalized;
            }
        } else if (state == 0)
        {
            if (confusionTimer <= 0)
            {
                state = 1;
            } else
            {
                confusionTimer -= Time.deltaTime;
            }
        }
        speed += speedScaling * Time.deltaTime / 60;
        acceleration += accelerationScaling * Time.deltaTime / 60;

    }

    void FixedUpdate()
    {
        Vector3 oldPos = transform.position;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        transform.LookAt(transform.position + direction, Vector3.up);
    }

    public void ChangeTarget(GameObject newTarget)
    {
        target = newTarget;
        //Change missile's material to new target's
        gameObject.GetComponentInChildren<Renderer>().material = newTarget.GetComponentInChildren<Renderer>().material;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!isServer) {return;}
        if (collider.gameObject.tag == "Player" && collider.gameObject.GetComponent<Player>().isAlive == true)
        {
            if (collider.gameObject.GetComponent<Player>().hasShield == true)
            {
                state = 0;
                direction = direction * -1;
                gameObject.GetComponentInChildren<Renderer>().material = missileMat;
                confusionTimer = confusionTime;
            } else
            {
                collider.gameObject.GetComponent<Player>().Die();
                collider.gameObject.GetComponent<Player>().isAlive = false;
                am.PlaySFX("playerDeath");
            }
            closestDist = 1000f;
            //Check if last player was killed
            if (!isAlivePlayer())
            {
                Debug.Log("no more players alive <3");
                gameManager.GetComponent<GameManager>().EndRound(collider.gameObject.GetComponent<Player>().pIndex);
            }
        } else if (collider.gameObject.tag == "Wall")
        {
            if (state != 0) {
            confusionTimer = confusionTime;
            }
            state = 0;
            direction = direction * -1;
            gameObject.GetComponentInChildren<Renderer>().material = missileMat;
            if (collider.gameObject.name != "Wall")
            {
                am.PlaySFX("missileHitWall");
                Destroy(collider.gameObject);
            }
        }
    }
    private bool isAlivePlayer() {
        foreach (GameObject player in players) {
            if (player.GetComponent<Player>().isAlive) {
                return true;
            }
        }
        return false;
    }
}
