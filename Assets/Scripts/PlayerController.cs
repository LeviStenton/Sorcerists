using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float maxSpeed = 10f;
    public float distToGround;
    private Transform myTransform;

    public LayerMask groundLayer;
    GameController gameController;

    public bool facingRight = true;
    public bool jumping = false;
    public bool isGrounded;

    public Vector2 jumpHighVec;
    public float jumpHighPow;
    public Vector2 jumpLongVec;
    public float jumpLongPow;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        distToGround = GetComponent<CapsuleCollider2D>().bounds.extents.y;

        myTransform = this.transform;
    }

    // Update is called once per frame
    void FixedUpdate() {
        Movement();
        Jumping();
        Grounded();        
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
        if (Input.GetButton("Horizontal") && isGrounded)
        {
            float move = Input.GetAxis("Horizontal");

            myTransform.position = new Vector2(transform.position.x + move * maxSpeed * Time.deltaTime, transform.position.y);
            //transform.position += move * maxSpeed * Time.deltaTime;

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
                Debug.Log("Jumping");
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(jumpHighVec * jumpHighPow, ForceMode2D.Impulse);
            }
            if (!facingRight)
            {
                Debug.Log("Jumping");
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(-jumpHighVec * -jumpHighPow, ForceMode2D.Impulse);
            }
        }

        if (Input.GetButtonDown("JumpLong") && isGrounded)
        {
            if (facingRight)
            {
                Debug.Log("Jumping");
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(jumpLongVec * jumpLongPow, ForceMode2D.Impulse);
            }
            if (!facingRight)
            {
                Debug.Log("Jumping");
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(-jumpLongVec * -jumpLongPow, ForceMode2D.Impulse);
            }
        }
    }

    public void Grounded()
    {
        
        if (Physics2D.Raycast(transform.position , Vector2.down, distToGround + 0.1f, groundLayer))
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
}
