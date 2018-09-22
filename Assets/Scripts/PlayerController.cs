using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

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
    Grounded groundedCheck;
    CircleCollider2D cirCol;
    public GameObject spellTarget;

    public GameObject arcMisSpell;

    public bool facingRight = true;
    public bool jumping = false;
    public bool isGrounded;

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

    public float playerHealth;
    public float spellChargeSpeed;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        enemyController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        groundedCheck = GetComponent<Grounded>();              

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

    public void TargetSystem()
    { 
        Instantiate(spellTarget, myTransform.position, myTransform.rotation);
        spellTargeting = GameObject.FindGameObjectWithTag("SpellTarget").GetComponent<SpellTargeting>();
    }

    IEnumerator StopPlayerFor(float delay)
    {
        canMove = false;
        yield return new WaitForSeconds(delay);
        canMove = true;
        Debug.Log("Wait Over");
    }

    void ArcaneMissileCast()
    {
        if(spellTargeting.arcaneMissiles && spellTargeting.targetConfirmed && spellTargeting.chargedUpSpell)
        {
            spellTargeting.spellOver = false;
            Instantiate(arcMisSpell, myTransform.position, myTransform.rotation);
            spellTargeting.arcaneMissiles = false;
            gameController.turnOver = true;
        }
    }
}
