using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float horizontalMove;
    [SerializeField] int currentDirection;
    private float verticalMove;
    [SerializeField] private bool isJumping;
    private bool isGrounded;
    [SerializeField] private bool isWallJumping;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isLanding;
    private bool isWalledLeft;
    private bool isWalledRight;
    [SerializeField] private bool isGrabbing;
    [SerializeField] private bool onWallLeft;
    [SerializeField] private bool onWallRight;
    [SerializeField] private bool canWallJump;
    [SerializeField] private bool towardWall;
    [SerializeField] private Vector2 forceWall;

    public Animator animator;
    private SpriteRenderer spriteRenderer;


    private Rigidbody2D rb;
    private float initialGravityScale; //gravite de depart
    private Vector2 zeroVelocity = Vector2.zero;

    [SerializeField] private float speed = 6f;
    [SerializeField] private float tempSlow = 2f;
    [SerializeField] private float tempSpeed;
    [SerializeField] private float timeSinceShoot = 0f;
    [SerializeField] private float freezeTime = 0f;


    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float velocityThreshold = 0.15f;
    [SerializeField] private float fallGravityMultiplier = 2.2f;
    [SerializeField] private float lowJumpGravityMultiplier = 2.5f;


    [SerializeField] private float wallJumpCounter;
    //[SerializeField] private float wallJumpTime = 0.2f;

    [SerializeField] private LayerMask wallLayers;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckHeight;
    [SerializeField] private float groundCheckWidth;

    [SerializeField] private Transform wallCheckLeft;
    [SerializeField] private float wallCheckLeftHeight;
    [SerializeField] private float wallCheckLeftWidth;
    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private float wallCheckRightHeight;
    [SerializeField] private float wallCheckRightWidth;


    private void Awake()
    {
        // On recupere le composant Rigidbody2D du player
        rb = GetComponent<Rigidbody2D>();

        // On memorise l'echelle de gravite de depart
        initialGravityScale = rb.gravityScale;

        // On recupere l'animateur du player
        animator = GetComponent<Animator>();

        // On recupere le composant SpriteRenderer du player
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        tempSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        //ENTREE MOOVE
        // On recupere une valeur qui vaudra -1 si on le joueur utilise la fleche gauche, 1 si il utilise la droite, et sinon 0
        horizontalMove = Input.GetAxisRaw("Horizontal");
        currentDirection = horizontalMove > 0 ? 1 : horizontalMove < 0 ? -1 : currentDirection;
        CheckCollider();
        //ENTREE GROUNDJUMP
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // On dit qu'on veut sauter
            isJumping = true;
        }

        //ENTREE WALLJUMP
        if (Input.GetButtonDown("Jump") && isGrabbing)
        {
            isWallJumping = true;
            Debug.Log(1);
        }

        //TIMER
        timeSinceShoot += Time.deltaTime;
        if (timeSinceShoot > freezeTime)
        {
            tempSpeed = speed;
        }
    }

    void FixedUpdate()
    {
        CheckJump();
        Moove();
        Jump();
        Grabbing();
        Fall();
        SpriteAnime();
        Decroche();
    }

    public void Moove()
    {
        float movementSmoothing = 0.2f;
        isLanding = false;
        //On cree un nouveau vecteur (-1;0;1*speed,la velocite du rigid body)
        Vector2 targetVelocity = new Vector2(horizontalMove * tempSpeed, rb.velocity.y);
        //SmoothDamp(position acuelle, position desiree, velocite,le temps pour atteindre la position)
        //la velocite du rigid body prend la valeur d'un nouveau vecteur
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref zeroVelocity, movementSmoothing);

        //
        float characterVelocity = Mathf.Abs(rb.velocity.x) <= 0.3f ? 0 : Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", characterVelocity);
        animator.SetBool("Landing", isLanding);
    }

    public void SlowWalk()
    {
        timeSinceShoot = 0f;

        if (timeSinceShoot < freezeTime)
        {
            timeSinceShoot = 0f;
            tempSpeed = tempSlow;
        }
    }

    public bool CheckJump()
    {
        // On test si une zone rectangulaire au niveau des pieds ("groundCheck") se superpose a un ou plusieurs elements du calque specifie ("Ground")
        if (Physics2D.OverlapBox(groundCheck.position, new Vector2(groundCheckWidth, groundCheckHeight), 0f, groundLayers) != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        return isGrounded;
    }
    
    
    public void Jump()
    {
        //si on veut sauter
        if (isJumping)
        {
            isFalling = false;
            //on change la velocite
            rb.velocity = Vector2.up * jumpForce;
            //Animation de saut
            animator.SetBool("Jumping", isJumping);
            animator.SetBool("Falling", isJumping);

            if (isFalling)
            {
                isJumping = false;
                animator.SetBool("Jumping", isJumping);
            }

            if (isGrabbing)
            {
                isJumping = false;
                animator.SetBool("Jumping", isJumping);
                isGrabbing = true;
                animator.SetBool("Grabbing", isGrabbing);
            }
            //isGrabbing = false;
        }
        isJumping = false;
    }

    public void Grabbing ()
    {
        if (!isGrounded && isGrabbing)
        {
            animator.SetBool("Grabbing", isGrabbing);
            isJumping = false;
            animator.SetBool("Jumping", isJumping);

            //la gravite est mise a 0 
            rb.gravityScale = 0f;
            //la velocite est mise a 0
            rb.velocity = Vector2.zero;

            //WallJump();

        }
        else
        {
            //on retablit la gravite originelle
            rb.gravityScale = initialGravityScale;
            isGrabbing = false;
            animator.SetBool("Grabbing", isGrabbing);
        }
    }
    public void Fall()
    {
        if (rb.velocity.y < -velocityThreshold)
        {
            //si le personnage tombe
            //on lance l'animation falling
            isWallJumping = false;
            isFalling = true;
            isJumping = false;
            animator.SetBool("Falling", isFalling);
            animator.SetBool("WallJumping", isWallJumping);
            animator.SetBool("Jumping", isJumping);
            //on modifie sa gravite pour tomber plus vite
            rb.gravityScale = initialGravityScale * fallGravityMultiplier;

            if (isGrounded) 
            {
                isLanding = true;
                animator.SetBool("Landing", isLanding);
            }
            isLanding = false;
        }
        else
        {
            //on ne lance pas l'animation 
            isFalling = false;
            animator.SetBool("Falling", isFalling);
            isLanding = false;
        }

        if (rb.velocity.y > velocityThreshold)
        {
            //si la personne monte
            if (Input.GetButton("Jump"))
            {
                //sa gravite reviens a celle de base
                rb.gravityScale = initialGravityScale;
            }
            else
            {
                //on augmente pour le jump
                rb.gravityScale = initialGravityScale * lowJumpGravityMultiplier;
            }
        }
    }

    public void SpriteAnime()
    {
        if(!isGrabbing)
        {
            spriteRenderer.flipX = horizontalMove > 0 ? false : horizontalMove < 0 ? true : spriteRenderer.flipX;
        }
        animator.SetBool("WallJumping", isWallJumping);
        animator.SetBool("Grabbing", isGrabbing);

    }
    // Si tu saute depuis un mur et que tu vas vers le mur sur lequel tu es, tu te fais expulser du mur sinon tu fais le wall jumping normal
    private void Decroche()
    {
        if (isWallJumping)
        {
            if (!canWallJump && towardWall)
            {
                rb.AddForce(new(-currentDirection * forceWall.x, forceWall.y));
            } else if(canWallJump && !towardWall)
            {
                if (horizontalMove > 0)
                {
                    // On ne fait pas de symétrie sur le sprite du player
                    spriteRenderer.flipX = true;
                } // Sinon ET Si on va à gauche
                else if (horizontalMove < 0)
                {
                    // On fait une symétrie sur le sprite du player
                    spriteRenderer.flipX = false;
                }
                //la velocite  est modifier
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, jumpForce);
                rb.gravityScale = initialGravityScale;
            }
        }
    }
     private void CheckCollider()
    {//isGrabbing = true si currentdirection direction est à D et qu'il touche le mur de D et inversement
        onWallLeft = Physics2D.OverlapBox(wallCheckLeft.position, new Vector2(wallCheckLeftWidth, wallCheckLeftHeight), 0f, wallLayers) != null;
        onWallRight = Physics2D.OverlapBox(wallCheckRight.position, new Vector2(wallCheckRightWidth, wallCheckRightHeight), 0f, wallLayers) != null;
        isGrabbing = onWallLeft || onWallRight;
        canWallJump = (onWallLeft && horizontalMove > 0) || (onWallRight && horizontalMove < 0);
        towardWall = (onWallLeft && currentDirection == -1) || (onWallRight && currentDirection == 1);
    }

    void OnDrawGizmos()
    {
        // Dessine un cube vert a la position du groundCheck
        Gizmos.color = new Color32(0, 255, 0, 90);
        Gizmos.DrawCube(groundCheck.position, new Vector2(groundCheckWidth, groundCheckHeight));
        Gizmos.DrawCube(wallCheckRight.position, new Vector2(wallCheckRightWidth, wallCheckRightHeight));
        Gizmos.DrawCube(wallCheckLeft.position, new Vector2(wallCheckLeftWidth, wallCheckLeftHeight));

    }


}
