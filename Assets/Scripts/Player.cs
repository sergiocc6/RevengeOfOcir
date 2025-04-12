using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    private float movement = 0f;
    public float moveSpeed = 7f;
    private bool facingRight = true;

    public float jumpHeight = 10f;
    public bool isGround = true;

    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal");

        if (movement < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && !facingRight)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        //Salto
        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            Jump();
            isGround = false;
            animator.SetBool("Jump", true);
        } 

        //Animaciones
        if(Math.Abs(movement) > .1f)
        {
            animator.SetFloat("Run", 1f);
        }
        else if(movement < .1f)
        {
            animator.SetFloat("Run", 0f);
        }

        //Ataque (click izquierdo)
        if(Input.GetMouseButtonDown(0))
        {            
            animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);

        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);
        }
    }
}
