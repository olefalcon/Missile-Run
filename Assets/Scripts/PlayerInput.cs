using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : NetworkBehaviour
{
    public GameObject gm;
    private Settings settings;
    public int playerIndex;
    private string upInput;
    private string rightInput;
    private string downInput;
    private string leftInput;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager");
        settings = gm.GetComponent<Settings>();
        if (playerIndex == 0) {
            upInput = settings.upInput;
            leftInput = settings.leftInput;
            downInput = settings.downInput;
            rightInput = settings.rightInput;  
        } else if (playerIndex == 1) {
            upInput = settings.p2UpInput;
            leftInput = settings.p2LeftInput;
            downInput = settings.p2DownInput;
            rightInput = settings.p2RightInput;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //No input if this isn't your player
        if (!isLocalPlayer) {return;}
        //No input before start delay
        if (!gm.GetComponent<GameManager>().isStart) {return;}
        //Default Direction Values
        float horizontal = 0;
        float vertical = 0;
        //Check for inputs
        if (Input.GetKey(upInput))
        {
            vertical += 1;
        }
        if (Input.GetKey(downInput))
        {
            vertical -= 1;
        }
        if (Input.GetKey(rightInput))
        {
            horizontal += 1;
        }
        if (Input.GetKey(leftInput))
        {
            horizontal -= 1;
        }
        direction = new Vector3(horizontal, 0f, vertical).normalized;
        gameObject.GetComponent<Player>().direction = direction;
    }
}
