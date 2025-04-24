using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    private float movement = 0f;
    public float moveSpeed = 7f;
    private bool facingRight = true;

    public float jumpSpeed = 10f;
    public float doubleJumpSpeed = 8f;
    public bool isGround = true;
    bool canDoubleJump;

    bool isTouchingFront = false;
    bool wallSliding;
    public float wallSlidingSpeed = 0.75f;
    bool isTouchingWallRight;
    bool isTouchingWallLeft;

    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal");

        if (movement < 0f && facingRight && !isTouchingWallRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && !facingRight && !isTouchingWallLeft)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        //Salto
        if (Input.GetKey(KeyCode.Space))
        {
            if (isGround)
            {
                animator.SetBool("Jump", true);
                canDoubleJump = true;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
                isGround = false;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animator.SetBool("Jump", false);
                    animator.SetBool("DoubleJump", true);
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpSpeed);
                    canDoubleJump = false;
                    isGround = false;
                }
            }
        }

        //Caer
        if (rb.linearVelocity.y < 0f)
        {
            animator.SetBool("Jump", false);
            if (!wallSliding)
                animator.SetBool("Fall", true);
        }
        else if (rb.linearVelocity.y > 0f)
        {
            animator.SetBool("Fall", false);
        }

        //Correr
        if (Math.Abs(movement) > .1f)
        {
            animator.SetFloat("Run", 1f);
        }
        else if (movement < .1f)
        {
            animator.SetFloat("Run", 0f);
        }

        //Ataque (click izquierdo)
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }

        //Deslizarse pared
        if (isTouchingFront && !isGround)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        if (wallSliding)
        {
            animator.Play("Player_Wall");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);

        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);
            animator.SetBool("DoubleJump", false);
            animator.SetBool("Fall", false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "RightWall")
        {
            isTouchingFront = true;
            isTouchingWallRight = true;
        }

        if (collision.gameObject.tag == "LeftWall")
        {
            isTouchingFront = true;
            isTouchingWallLeft = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isTouchingFront = false;
        isTouchingWallRight = false;
        isTouchingWallLeft = false;
    }
}
