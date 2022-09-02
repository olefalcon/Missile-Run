using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : NetworkBehaviour
{
    public Vector3 targetLocation;
    public float speed = 2f;
    public GameManager gm;

    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        ChooseTargetLoc();
    }

    // Update is called once per frame
    void Update()
    {
        //Ai does not calculate on clients
        if (!isServer) {return;}
        //Ai does not start until the start delay has passed
        if (!gm.isStart) {return;}
        if (Vector3.Distance(transform.position, targetLocation) <= 0.1f)
        {
            ChooseTargetLoc();
        }
        direction = (targetLocation - transform.position).normalized;
        gameObject.GetComponent<Player>().direction = direction;
    }

    public void ChooseTargetLoc()
    {
        targetLocation = new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));
    }
}
