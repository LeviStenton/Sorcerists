using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneBomb : MonoBehaviour
{
    //public Sprite bombEffect;
    //public float bombEffectLifeTime = 1.0f;

    public float lifeTime;
    public float damage;
    public float power;
    public float radius;
    public float spellMoveSpeed;
    public int bounceTimes;

    bool tracking;
    bool canBounce;

    public Vector3 spellTarget;
    public Vector3 spellLauDir;
    Quaternion targetRot;

    GameController gameController;
    EnemyController enemyController;
    PlayerController playerController;
    SpellTargeting spellTargeting;

    LayerMask groundLayer;

    private TrailRenderer trail;
    Rigidbody2D rb;

    //public AudioClip hitSound;
    //private AudioSource source;

    void Awake()
    {
        //spellTarget = GameObject.FindGameObjectWithTag("SpellTarget").transform.position;
        trail = GetComponent<TrailRenderer>();
        //source = GetComponent<AudioSource>();
        StartCoroutine(happenAfterTime());
        Physics2D.IgnoreLayerCollision(10, 2, true);
    }


    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemyController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
        rb = GetComponent<Rigidbody2D>();
        spellLauDir = playerController.spellLauDir;
        spellMoveSpeed = playerController.spellMoveSpeed;
        canBounce = true;
    }

    void Update()
    {
        if(canBounce)
            transform.position += spellLauDir * (spellMoveSpeed * 3f) * Time.deltaTime;

        Physics2D.IgnoreLayerCollision(10, 2, true);

        Debug.Log(rb.sharedMaterial.bounciness);
    }

    void OnTriggerEnter2D(Collider2D otherObject)
    {
        /*if (otherObject.tag == "Enemy")
        {
            //public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, int layerMask = DefaultRaycastLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity);
            otherObject.transform.GetComponent<EnemyController>().takeDamage(damage);
            //Instantiate(missileEffect, transform.position, transform.rotation);
            //source.PlayOneShot(hitSound, 2);
            foreach (Transform child in transform)
            {
                GameObject.Destroy(this.gameObject);
            }
            transform.position = Vector3.one * 9999999f;
            Destroy(gameObject, hitSound.length);
            playerController.spellOver = true;
        }
        if (otherObject.tag == "Ground")
        {
            //Instantiate(missileEffect, transform.position, transform.rotation);
            //source.PlayOneShot(hitSound, 2);
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            transform.position = Vector3.one * 9999999f;
            Destroy(gameObject, hitSound.length);
            playerController.spellOver = true;
        }*/
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Enemy")
        {
            if(bounceTimes >= 1)
            {
                rb.gravityScale += 5;
                bounceTimes -= 1;
                Debug.Log("Bouncing");
            }
            if(bounceTimes <= 0)
            {
                canBounce = false;
                StartCoroutine(happenAfterTime());
                bounceTimes = 0;
            }
        }        
    }

    IEnumerator happenAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        playerController.spellOver = true;
        //Instantiate(missileEffect, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
