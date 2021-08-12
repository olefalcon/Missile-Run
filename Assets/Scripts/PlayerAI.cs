using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public GameObject missile;
    public Vector3 targetLocation;
    public float speed = 2f;

    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        missile = GameObject.Find("Missile");
        ChooseTargetLoc();
    }

    // Update is called once per frame
    void Update()
    {
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
