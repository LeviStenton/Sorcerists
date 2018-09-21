using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneMissiles : MonoBehaviour {

    public float rotationSpeed;
    
    //public Sprite missileEffect;
    //public float missileEffectLifeTime = 1.0f;
        
    public float lifeTime;
    public float damage;

    Vector3 spellTarget;

    GameController gameController;
    EnemyController enemyController;
    PlayerController playerController;
    SpellTargeting spellTargeting;

    private TrailRenderer trail;

    //public AudioClip hitSound;
    //private AudioSource source;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemyController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
        spellTargeting = GameObject.FindGameObjectWithTag("SpellTarget").GetComponent<SpellTargeting>();
        spellTarget = GameObject.FindGameObjectWithTag("SpellTarget").transform.position;
    }

    //Use this for initialization
    void Awake()
    {
        //trail = GetComponent<TrailRenderer>();
        //source = GetComponent<AudioSource>();

        StartCoroutine(happenAfterTime());

        Movement();
    }

    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        Debug.Log(transform.position);

        transform.position += transform.up * spellTargeting.spellMoveSpeed * Time.deltaTime;
        
        //Missle Guidance
        if (spellTarget != null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation (transform.position, spellTarget - transform.position),Time.deltaTime);

            //Quaternion targetRotation = Quaternion.LookRotation(spellTarget - transform.position);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

    }

    void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.tag == "Enemy")
        {
            otherObject.transform.GetComponent<EnemyController>().takeDamage(damage);
            //Instantiate(missileEffect, transform.position, transform.rotation);
            //source.PlayOneShot(hitSound, 2);
            foreach (Transform child in transform)
            {
                GameObject.Destroy(this.gameObject);
                spellTargeting.spellOver = true;
            }
            transform.position = Vector3.one * 9999999f;
            Destroy(gameObject/*, hitSound.length*/);
            spellTargeting.spellOver = true;
        }
        if (otherObject.tag == "Ground")
        {
            //Instantiate(missileEffect, transform.position, transform.rotation);
            //source.PlayOneShot(hitSound, 2);
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
                spellTargeting.spellOver = true;
            }
            transform.position = Vector3.one * 9999999f;
            Destroy(gameObject/*, hitSound.length*/);
            spellTargeting.spellOver = true;
        }
    }

    IEnumerator happenAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(this.gameObject);
        spellTargeting.spellOver = true;
        //Instantiate(missileEffect, transform.position, transform.rotation);
    }
}
