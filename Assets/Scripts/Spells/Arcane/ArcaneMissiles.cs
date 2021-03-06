﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneMissiles : MonoBehaviour {

    public float rotationSpeed;
    
    //public Sprite missileEffect;
    //public float missileEffectLifeTime = 1.0f;
        
    public float lifeTime;
    public float damage;
    public float spellMoveSpeed;

    bool tracking;

    public Vector3 spellTarget;
    public Vector3 spellLauDir;
    Quaternion targetRot;

    GameController gameController;
    EnemyController enemyController;
    PlayerController playerController;
    SpellTargeting spellTargeting;

    private TrailRenderer trail;

    //public AudioClip hitSound;
    //private AudioSource source;

    void Awake()
    {
        spellTarget = GameObject.FindGameObjectWithTag("SpellTarget").transform.position;
        trail = GetComponent<TrailRenderer>();
        //source = GetComponent<AudioSource>();
        tracking = false;
        StartCoroutine(happenAfterTime());        
    }


    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemyController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
        spellTargeting = GameObject.FindGameObjectWithTag("SpellTarget").GetComponent<SpellTargeting>();
        spellLauDir = playerController.spellLauDir;
        spellMoveSpeed = playerController.spellMoveSpeed;        
        StartCoroutine(delayTracking());
    }

    void Update()
    {
        if (!tracking)
            transform.position += spellLauDir * spellMoveSpeed * Time.deltaTime;

        else if(tracking)
            transform.position += transform.up * spellMoveSpeed * Time.deltaTime;

        if (spellTarget != null)
        {
            targetRot = Quaternion.FromToRotation(transform.position, spellTarget - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
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
            }
            transform.position = Vector3.one * 9999999f;
            Destroy(gameObject/*, hitSound.length*/);
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
            Destroy(gameObject/*, hitSound.length*/);
            playerController.spellOver = true;
        }
    }

    IEnumerator delayTracking()
    {
        yield return new WaitForSeconds((spellMoveSpeed * Time.deltaTime) * 5);
        tracking = true;
    }

    IEnumerator happenAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(this.gameObject);
        playerController.spellOver = true;
        //Instantiate(missileEffect, transform.position, transform.rotation);
    }
}
