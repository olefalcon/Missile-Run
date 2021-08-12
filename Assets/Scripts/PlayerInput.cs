using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public GameObject gameManager;
    private Settings settings;
    private string upInput;
    private string rightInput;
    private string downInput;
    private string leftInput;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        settings = gameManager.GetComponent<Settings>();
        upInput = settings.upInput;
        leftInput = settings.leftInput;
        downInput = settings.downInput;
        rightInput = settings.rightInput;
    }

    // Update is called once per frame
    void Update()
    {
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
