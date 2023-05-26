using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask whatisGround;
    [SerializeField] private LayerMask whatisGrabbing;

    private bool isGrounded;

    public Animator anim;

    [SerializeField] private Transform wallGrabPoint;
    private bool canGrab, isGrabbing;
    private float gravityStore;
    [SerializeField] private float wallJumpCounter;
    [SerializeField] private float wallJumpTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        gravityStore = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(wallJumpCounter <=0)
        {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);

            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatisGround);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            //flip direction
            if (rb.velocity.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1, 1f);
            }
            
            //handle wall jumping
            canGrab = Physics2D.OverlapCircle(wallGrabPoint.position, .2f, whatisGrabbing);

            isGrabbing = false;
            if(canGrab && !isGrounded)
            {
                if((transform.localScale.x == 1f && Input.GetAxisRaw("Horizontal")>0) || (transform.localScale.x == -1f && Input.GetAxisRaw("Horizontal")<0))
                {
                    isGrabbing = true;
                }
            }

            if(isGrabbing)
            {
                rb.gravityScale = 0f;
                rb.velocity = Vector2.zero;

                if(Input.GetButtonDown("Jump"))
                {
                    wallJumpCounter = wallJumpTime;

                    rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed,jumpForce);
                    rb.gravityScale = gravityStore;
                    isGrabbing = false;
                }
            }else
            {
                rb.gravityScale = gravityStore;
            }

        }else
        {
            wallJumpCounter -= Time.deltaTime;
        }

        //anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
        //anim.SetBool("isGrounded", isGrounded);
    }
}