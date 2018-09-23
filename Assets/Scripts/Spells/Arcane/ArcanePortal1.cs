using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcanePortal1 : MonoBehaviour {

    PlayerController playerController;
    
    ArcanePortal2 portal2;

	// Use this for initialization
	void Start ()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

    void Update()
    {
        portal2 = GameObject.FindGameObjectWithTag("ArcaneExit").GetComponent<ArcanePortal2>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !playerController.hasPortalled2)
        {
            playerController.transform.position = portal2.transform.position;
            playerController.hasPortalled1 = true;
            Debug.Log("Enter1");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerController.hasPortalled2 = false;
            Debug.Log("Exit1");
        }
    }
}
