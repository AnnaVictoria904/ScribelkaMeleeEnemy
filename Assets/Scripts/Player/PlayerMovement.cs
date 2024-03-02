using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Animator animator;
    private BoxCollider2D coll;
    private AudioSource audioSource;
    [SerializeField] private AudioClip jump;
    //[SerializeField] private LayerMask jumpableGround;
    //private int doubleJump = 2;
    public float axisH;
    private float wallJumpCooldown;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("PlayerMovement without Rigidbody2D");
        }
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("PlayerMovement without Animator");
        }
        coll = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        PlayerPrefs.SetInt("died", 0);
    }

    // Update is called once per frame
    void Update()
    {
        axisH = Input.GetAxis("Horizontal");
        //rb.velocity = new Vector2(axisH * speed, rb.velocity.y);

        if (axisH > 0)
        {
            transform.localScale = Vector3.one;
        }

        else if (axisH < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        animator.SetBool("running", axisH != 0);
        animator.SetBool("grounded", isGrounded());

        /*if (Input.GetKeyDown(KeyCode.Space) && doubleJump < 2)
        {
            doubleJump++;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("jumped", true);
        }*/

        //Wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            rb.velocity = new Vector2(axisH * speed, rb.velocity.y);

            if (onWall() && !isGrounded())
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }
            else
                rb.gravityScale = 5;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("jumped", false);
            doubleJump = 0;
        }
    }*/

    /*private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }*/
    private void Jump()
    {
        animator.SetTrigger("jumped");
        if (isGrounded())
        {
            audioSource.PlayOneShot(jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (onWall() && !isGrounded())
        {
            if (axisH == 0)
            {
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            audioSource.PlayOneShot(jump);
            wallJumpCooldown = 0;
        }
    }
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return isGrounded() && !onWall() && axisH == 0;
    }
}
