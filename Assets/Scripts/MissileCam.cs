using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCam : MonoBehaviour
{
    public float rotateSpeed;
    public float skyboxColorSpeed;
    public Camera cam;
    void Start() {
        cam = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, rotateSpeed*Time.deltaTime, 0f),Space.Self);
        float t = Mathf.PingPong(Time.time, skyboxColorSpeed)/skyboxColorSpeed;
        cam.backgroundColor = Color.Lerp(Color.red, Color.blue, t);
    }
}
