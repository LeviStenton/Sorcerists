using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUpRotate : MonoBehaviour {

    float chargeRotSpeed = 100f;

    public Slider chargeUp;
    PlayerController playerController;
    SpellTargeting spellTargeting;

    // Use this for initialization
    void Start ()
    {
        chargeUp = GetComponentInChildren<Slider>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        spellTargeting = GameObject.FindGameObjectWithTag("SpellTarget").GetComponent<SpellTargeting>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, chargeRotSpeed * Time.deltaTime);

        ChargeUpSpell();
    }

    void ChargeUpSpell()
    {
        if (Input.GetButton("LeftMouse"))
        {
            chargeUp.value += playerController.spellChargeSpeed * Time.deltaTime;
        }
        if (Input.GetButtonUp("LeftMouse") || chargeUp.value == chargeUp.maxValue)
        {
            spellTargeting.chargedUpSpell = true;
            Destroy(this.gameObject);
        }
    }
}

