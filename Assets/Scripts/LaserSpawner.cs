using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    //Prefab for spawning laser
    public GameObject laserPrefab;
    //variable for laser spawning
    public float laserSpawnTime;
    private float laserSpawnTimer;
    //variables for laser creation
    public float minLaserSpeed;
    public float maxLaserSpeed;
    public float minLaserLength;
    public float maxLaserLength;
    //materials for laser
    public Material mat1;
    public Material mat2;
    public Material mat3;
    public Material mat4;

    // Start is called before the first frame update
    void Start()
    {
        CreateLaser();
        laserSpawnTimer = laserSpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (laserSpawnTimer <= 0) {
            CreateLaser();
            laserSpawnTimer = laserSpawnTime;
        } else {
            laserSpawnTimer -= Time.deltaTime;
        }
    }

    void CreateLaser() {
        float laserSpeed = Random.Range(minLaserSpeed, maxLaserSpeed);
        float laserLength = Random.Range(minLaserLength, maxLaserLength);
        float laserZ = Random.Range(-15f,15f);
        GameObject laser = Instantiate(laserPrefab, transform.position + Vector3.forward*laserZ, transform.rotation);
        laser.GetComponent<Laser>().speed = laserSpeed;
        laser.transform.localScale = new Vector3(laserLength, .01f, 0.1f);
        laser.GetComponentInChildren<Renderer>().material = PickMat();
    }

    Material PickMat() {
        int mat = Random.Range(1,5);
        switch (mat) {
            case 1:
                return mat1;
            case 2:
                return mat2;
            case 3:
                return mat3;
            case 4:
                return mat4;
        }
        return mat1;
    }
}
