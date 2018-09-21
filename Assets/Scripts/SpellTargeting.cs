using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTargeting : MonoBehaviour {

    Transform myTransform;
    PlayerController playerController;
    CameraController cameraController;
    ChargeUpRotate chargeUpRot;
    public GameObject spellChargeRot;
    public GameObject player;


    public float chargeRotSpeed;
    public float spellMoveSpeed;

    public bool arcaneMissiles = false;

    public bool targetConfirmed = false;
    public bool spellOver = false;
    public bool chargedUpSpell = false;

	// Use this for initialization
	void Start ()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();        

        player = GameObject.FindGameObjectWithTag("Player");
        myTransform = this.transform;
        playerController.canMove = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        ConfirmTarget();

        Vector3 temp = Input.mousePosition;
        temp.z = 10;        

        if(!targetConfirmed)
            myTransform.position = Camera.main.ScreenToWorldPoint(temp);

        if (targetConfirmed)
        {
            myTransform.position = GameObject.FindGameObjectWithTag("SpellTarget").transform.position;            
            cameraController.FocusTarget(player);
            Instantiate(spellChargeRot, playerController.transform.position, playerController.transform.rotation);
            chargeUpRot = GameObject.FindGameObjectWithTag("SpellCharger").GetComponent<ChargeUpRotate>();
        }

        if (chargedUpSpell)
        {
            spellMoveSpeed = chargeUpRot.chargeUp.value;
        }

        if (spellOver)
        {
            playerController.canMove = true;
            Destroy(this.gameObject);
        }
    }

    void ConfirmTarget()
    {
        if (Input.GetButtonDown("LeftMouse"))
        {
            targetConfirmed = true;
        }
    }   

    public void arcaneMissilesOn()
    {
        arcaneMissiles = true;
    }
}
