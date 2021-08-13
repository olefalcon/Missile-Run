using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float startDelay;
    private float startDelayTimer;
    public GameObject gameManager;
    public Material missileMat;
    public float speed;
    public float acceleration;
    public float speedScaling;
    public float accelerationScaling;
    public GameObject[] players;
    public GameObject target;
    //State 0 equals not chasing
    //state 1 equals chasing
    //state 2 equals starting delay
    //state 3 equals no targets roam
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
        GameManager gameManagerScript = gameManager.GetComponent<GameManager>();
        players = gameManagerScript.players;
        int startTarget = Random.Range(0,4);
        target = players[startTarget];
        direction = Vector3.zero;
        state = 2;
        startDelayTimer = startDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 2)
        {
            if (startDelayTimer <= 0)
            {
                state = 1;
            } else
            {
                startDelayTimer -= Time.deltaTime;
            }
        }
        else if (state == 1)
        {
            if (target.GetComponent<Player>().isAlive == true)
            {
                closestDist = Vector3.Distance(target.transform.position, transform.position);
            }
            foreach (GameObject player in players)
            {
                float newDist = Vector3.Distance(player.transform.position, transform.position);
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
        if (state == 1) {
            transform.LookAt(target.transform, Vector3.up);
        }
        
    }

    public void ChangeTarget(GameObject newTarget)
    {
        target = newTarget;
        //Change missile's material to new target's
        gameObject.GetComponentInChildren<Renderer>().material = newTarget.GetComponentInChildren<Renderer>().material;
    }

    private void OnTriggerEnter(Collider collider)
    {
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
            }
            closestDist = 1000f;
            //Check if last player was killed
            if (players[0].GetComponent<Player>().isAlive == false && players[1].GetComponent<Player>().isAlive == false && players[2].GetComponent<Player>().isAlive == false && players[3].GetComponent<Player>().isAlive == false)
            {
                gameManager.GetComponent<GameManager>().EndRound(collider.gameObject.GetComponent<Player>().pIndex);
            }
        } else if (collider.gameObject.tag == "Wall")
        {
            state = 0;
            direction = direction * -1;
            gameObject.GetComponentInChildren<Renderer>().material = missileMat;
            confusionTimer = confusionTime;
            if (collider.gameObject.name != "Wall")
            {
                Destroy(collider.gameObject);
            }
        }
    }
}
