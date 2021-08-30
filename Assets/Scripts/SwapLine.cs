using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapLine : MonoBehaviour
{
    public float dieDelay;
    public float dieDelayTimer;
    public Transform p1t;
    public Transform p2t;
    void Start() {
        dieDelayTimer = dieDelay;
    }
    // Update is called once per frame
    void Update()
    {
        if (dieDelayTimer <= 0) {
            Destroy(gameObject);
        } else {
            dieDelayTimer -= Time.deltaTime;
        }
        if (p1t == null || p2t == null) {
            Destroy(gameObject);
        }
    }
    public void SetPos() {
        transform.position = (p1t.position + p2t.position)/2;
        transform.LookAt(p1t, Vector3.up);
        float length = Vector3.Distance(p1t.position, p2t.position);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, length);
    }
}
