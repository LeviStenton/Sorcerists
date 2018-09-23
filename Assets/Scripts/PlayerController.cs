using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float playerHealth;
    public float spellChargeSpeed;
    public float moveSpeed;
    float gravity = -20;
    Vector3 velocity;
    public float distToGround;
    private Transform myTransform;
    public bool canMove = true;

    public LayerMask groundLayer;
    GameController gameController;
    EnemyController enemyController;
    PlayerController playerController;
    SpellTargeting spellTargeting;
    ChargeUpRotate chargeUpRot;
    CameraController cameraController;
    Grounded groundedCheck;
    CircleCollider2D cirCol;
    public GameObject spellTarget;
    public GameObject spellChargeRot;

    public GameObject arcMisSpell;
    public GameObject arcBombSpell;
    public GameObject arcPortal1;
    public GameObject arcPortal2;

    public bool facingRight = true;
    public bool jumping = false;
    public bool isGrounded;
    public bool targetAndCharge;
    public bool hasPortalled1;
    public bool hasPortalled2;

    public bool arcaneMissiles = false;
    public bool arcaneBomb = false;
    public bool arcaneTeleport = false;
    public bool arcanePortal = false;

    const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    float verticalRaySpacing;
    RaycastOrigins raycastOrigins;

    public Vector2 jumpHighVecLeft;
    public Vector2 jumpHighVecRight;
    public float jumpHighPow;
    public Vector2 jumpLongVecLeft;
    public Vector2 jumpLongVecRight;
    public float jumpLongPow;
    public float jumpHighRecDelay;
    public float jumpLongRecDelay;

    public Vector3 spellLauDir;
    public float chargeRotSpeed;
    public float spellMoveSpeed;
    public float missileShootSpeed;

    public bool targetConfirmed = false;
    public bool spellOver = false;
    public bool chargedUpSpell = false;
    public bool readyToCharge = false;
    public bool nonChargeUpSpell = false;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        enemyController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        groundedCheck = GetComponent<Grounded>();
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        distToGround = GetComponent<CircleCollider2D>().bounds.extents.y;
        cirCol = GetComponent<CircleCollider2D>();

        myTransform = this.transform;
    }

    // Update is called once per frame
    void Update() {
        Movement();
        Jumping();
        Grounded();
        PlayerDeath();               

        if (Input.GetButtonDown("TestButton"))
            TakeDamage(10);
        if(spellTargeting != null)
            ArcaneMissileCast();
        ArcaneBombCast();
        ArcaneTeleportCast();
        ArcanePortalCast();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Movement()
    {
        if (canMove)
        {
            velocity.y = 0;


            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            velocity.x = input.x * moveSpeed;
            velocity.y += gravity * Time.deltaTime;
            groundedCheck.Move(velocity * Time.deltaTime);

            float move = Input.GetAxis("Horizontal");

            //myTransform.position = new Vector2(transform.position.x + move * maxSpeed * Time.deltaTime, transform.position.y);

            if (move > 0 && !facingRight)
                Flip();
            else if (move < 0 && facingRight)
                Flip();

        }        
    }

    void Jumping()
    {
        if (Input.GetButtonDown("JumpHigh") && isGrounded)
        {
            if (facingRight)
            {
                //Debug.Log("Jumping");
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(jumpHighVecRight * jumpHighPow, ForceMode2D.Impulse);
                StartCoroutine(StopPlayerFor(jumpHighRecDelay));
            }
            else if (!facingRight)
            {
                //Debug.Log("Jumping");
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(jumpHighVecLeft * jumpHighPow, ForceMode2D.Impulse);
                StartCoroutine(StopPlayerFor(jumpHighRecDelay));
            }
        }

        if (Input.GetButtonDown("JumpLong") && isGrounded)
        {
            if (facingRight)
            {
                //Debug.Log("Jumping");
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(jumpLongVecRight * jumpLongPow, ForceMode2D.Impulse);
                StartCoroutine(StopPlayerFor(jumpLongRecDelay));
            }
            else if (!facingRight)
            {
                //Debug.Log("Jumping");
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(jumpLongVecLeft * jumpLongPow, ForceMode2D.Impulse);
                StartCoroutine(StopPlayerFor(jumpLongRecDelay));
            }
        }
    }

    public void Grounded()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, distToGround + 0.1f, groundLayer))
        {
            //Debug.Log("Grounded");
            isGrounded = true;
        }
        else
        {
            //Debug.Log("Not Grounded");
            isGrounded = false;
        }
    }  

    void UpdateRaycastOrigins()
    {
        Bounds bounds = cirCol.bounds;
        bounds.Expand (skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.topRight = new Vector2(bounds.min.x, bounds.min.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = cirCol.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public void PlayerDeath()
    {
        if(playerHealth <= 0)
        {
            playerHealth = 0;
            if(playerHealth == 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    
    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
    }

    public void noTargetAndCharge()
    {
        targetAndCharge = false;
    }

    public void yesTargetAndCharge()
    {
        targetAndCharge = true;
    }

    public void TargetSystem()
    {
        if (targetAndCharge)
        {
            Instantiate(spellTarget, myTransform.position, myTransform.rotation);
            spellTargeting = GameObject.FindGameObjectWithTag("SpellTarget").GetComponent<SpellTargeting>();
        }

        if (!targetAndCharge)
        {
            canMove = false;
            Instantiate(spellChargeRot, playerController.transform.position, playerController.transform.rotation);
            chargeUpRot = GameObject.FindGameObjectWithTag("SpellCharger").GetComponent<ChargeUpRotate>();
            readyToCharge = false;
        }        
    }

    IEnumerator StopPlayerFor(float delay)
    {
        canMove = false;
        yield return new WaitForSeconds(delay);
        canMove = true;
        Debug.Log("Wait Over");
    }
    public void nonChargeUpSpellOn()
    {
        nonChargeUpSpell = true;
    }

    public void arcaneMissilesOn()
    {
        arcaneMissiles = true;
    }
    void ArcaneMissileCast()
    {
        if(arcaneMissiles && targetConfirmed && chargedUpSpell)
        {
            spellOver = false;
            Instantiate(arcMisSpell, myTransform.position, myTransform.rotation);
            arcaneMissiles = false;
            gameController.turnOver = true;
        }
    }

    public void arcaneBombOn()
    {
        arcaneBomb = true;
    }
    void ArcaneBombCast()
    {
        if(arcaneBomb && chargedUpSpell)
        {
            spellOver = false;
            Instantiate(arcBombSpell, myTransform.position, myTransform.rotation);
            arcaneBomb = false;
            gameController.turnOver = true;
        }
    }

    public void arcaneTeleportOn()
    {
        arcaneTeleport = true;
    }
    void ArcaneTeleportCast()
    {
        if(arcaneTeleport && targetConfirmed)
        {
            spellOver = false;
            transform.position = spellTargeting.transform.position;
            arcaneTeleport = false;
            gameController.turnOver = true;
            Destroy(spellTargeting.gameObject);
        }
    }

    public void arcanePortalOn()
    {
        arcanePortal = true;
    }
    void ArcanePortalCast()
    {
        if(arcanePortal && targetConfirmed)
        {
            spellOver = false;
            Vector3 aboveSpawn = new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z);
            Instantiate(arcPortal2, spellTargeting.transform.position, transform.rotation);
            Instantiate(arcPortal1, aboveSpawn, transform.rotation);
            cameraController.FocusTarget(arcPortal2);
            arcanePortal = false;
            gameController.turnOver = true;
            canMove = true;
        }
    }

    IEnumerator WaitFor(float delay)
    {
        yield return new WaitForSeconds(delay * Time.deltaTime);
    }
}
