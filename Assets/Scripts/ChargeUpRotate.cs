using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUpRotate : MonoBehaviour {

    float chargeRotSpeed = 100f;

    public Vector3 spellLauDir;

    public Slider chargeUp;
    PlayerController playerController;
    GameController gameController;
    SpellTargeting spellTargeting;

    public bool canRot;

    // Use this for initialization
    void Start ()
    {
        canRot = true;
        chargeUp = GetComponentInChildren<Slider>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        spellTargeting = GameObject.FindGameObjectWithTag("SpellTarget").GetComponent<SpellTargeting>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();        
}
	
	// Update is called once per frame
	void Update ()
    {
        if (canRot)
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, chargeRotSpeed * Time.deltaTime);
        }
        //spellLauDir = this.transform.up;
        ChargeUpSpell();
    }

    void ChargeUpSpell()
    {
        if (Input.GetButton("LeftMouse"))
        {
            canRot = false;
            chargeUp.value += playerController.spellChargeSpeed * (Time.deltaTime * 2);            
        }

        if (Input.GetButtonUp("LeftMouse") || chargeUp.value == chargeUp.maxValue)
        {
            spellTargeting.chargedUpSpell = true;
            spellLauDir = this.transform.up;
            Destroy(this.gameObject);           
        }

    }
}

