using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Powerup : NetworkBehaviour
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
    public float swapDelay;
    public GameObject swapLinePrefab;
    public float shieldTime;
    private float lifespanTimer;
    [SyncVar]
    public int powerupType;
    //materials for powerup
    public Material speedMat;
    public Material invisMat;
    public Material swapMat;
    public Material glitchMat;
    public Material shieldMat;
    //images for powerup
    public Texture speedImage;
    public Texture invisImage;
    public Texture swapImage;
    public Texture glitchImage;
    public Texture shieldImage;
    //renderer
    public Renderer rend;
    //image overlay
    public RawImage icon;
    // Start is called before the first frame update
    void Start()
    {
        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        lifespanTimer = lifespan;
        if (isServer) {
            AssignType();
        }
        AssignMat();
        StartTween();
    }

    // Update is called once per frame
    void Update()
    {
        if (lifespanTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    void StartTween() {
        icon.rectTransform.DOScale(2f, 0.46875f).SetLoops(-1, LoopType.Restart).SetEase(Ease.OutQuint);
        transform.DOScale(0.4f, 0.46875f).SetLoops(-1, LoopType.Restart).SetEase(Ease.OutQuint);
        transform.DOLocalRotate(new Vector3(0f,45f,0f), 0.46875f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.OutQuint);
    }

    void AssignType()
    {
        powerupType = Mathf.RoundToInt(Random.Range(1f, 5f));
    }

    public void AssignMat()
    {
        rend = gameObject.GetComponent<Renderer>();
        if (powerupType == 1)
        {
            rend.material = speedMat;
            icon.texture = speedImage;
        } else if (powerupType == 2)
        {
            rend.material = swapMat;
            icon.texture = swapImage;
        } else if (powerupType == 3)
        {
            rend.material = invisMat;
            icon.texture = invisImage;
        } else if (powerupType == 4)
        {
            rend.material = glitchMat;
            icon.texture = glitchImage;
        } else if (powerupType == 5)
        {
            rend.material = shieldMat;
            icon.texture = shieldImage;
        }
    }
    [ClientRpc]
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
                    GameObject swapLine = Instantiate(swapLinePrefab, Vector3.zero, Quaternion.identity);
                    swapLine.GetComponent<SwapLine>().p1t = player.transform;
                    swapLine.GetComponent<SwapLine>().p2t = farthestPlayer.transform;
                    swapLine.GetComponent<SwapLine>().SetPos();
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
