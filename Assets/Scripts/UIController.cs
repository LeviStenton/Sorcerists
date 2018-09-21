using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIController : MonoBehaviour {

    PlayerController playerController;
    EnemyController enemyController;

    public GameObject spellSelection;
    public GameObject spellSelHighPos;
    public GameObject spellSelLowPos;

    public Text playerHealthText;
    public Text enemyHealthText;

	// Use this for initialization
	void Start ()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemyController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        playerHealthText.text = "Player Health " + (int)playerController.playerHealth;
        enemyHealthText.text = "Enemy Health " + (int)enemyController.enemyHealth;
    }

   void LowerSpellSelect()
    {
        spellSelection.transform.position = spellSelLowPos.transform.position;
    }

    void HighSpellSelect()
    {
        spellSelection.transform.position = spellSelHighPos.transform.position;
    }
}
