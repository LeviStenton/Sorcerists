using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTargeting : MonoBehaviour {

    Transform myTransform;
    PlayerController playerController;
    CameraController cameraController;
    GameController gameController;
    ChargeUpRotate chargeUpRot;
    public GameObject spellChargeRot;
    public GameObject player;

    //public Vector3 spellLauDir;   

	// Use this for initialization
	void Start ()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        player = GameObject.FindGameObjectWithTag("Player");
        myTransform = this.transform;
        playerController.canMove = false;
        playerController.readyToCharge = false;
        if (playerController.arcaneTeleport)
            this.transform.localScale = playerController.transform.localScale;
    }
	
	// Update is called once per frame
	void Update ()
    {
        ConfirmTarget();

        Vector3 temp = Input.mousePosition;
        temp.z = 10;        

        if(!playerController.targetConfirmed)
            myTransform.position = Camera.main.ScreenToWorldPoint(temp);

        if (playerController.targetConfirmed)
        {
            myTransform.position = GameObject.FindGameObjectWithTag("SpellTarget").transform.position;            
            cameraController.FocusTarget(player);
            if (playerController.readyToCharge && !gameController.turnOver && !playerController.chargedUpSpell && !playerController.nonChargeUpSpell)
            {
                Instantiate(spellChargeRot, playerController.transform.position, playerController.transform.rotation);
                chargeUpRot = GameObject.FindGameObjectWithTag("SpellCharger").GetComponent<ChargeUpRotate>();
                playerController.readyToCharge = false;
            }
            else if (playerController.readyToCharge && !gameController.turnOver && !playerController.chargedUpSpell && playerController.nonChargeUpSpell)
            {
                playerController.readyToCharge = false;                
            }
        }        
    }

    void ConfirmTarget()
    {
        if (Input.GetButtonUp("LeftMouse"))
        {
            playerController.targetConfirmed = true;
            playerController.readyToCharge = true;
        }
    }     
}
