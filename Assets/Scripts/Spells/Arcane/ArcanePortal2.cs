using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcanePortal2 : MonoBehaviour {

    PlayerController playerController;

    ArcanePortal1 portal1;

	// Use this for initialization
	void Start ()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

    void Update()
    {
        portal1 = GameObject.FindGameObjectWithTag("ArcaneEntrance").GetComponent<ArcanePortal1>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !playerController.hasPortalled1)
        {
            playerController.transform.position = portal1.transform.position;
            playerController.hasPortalled2 = true;
            Debug.Log("Enter2");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerController.hasPortalled1 = false;
            Debug.Log("Exit2");
        }
    }
}
