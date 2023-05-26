using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolAndChase : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float jumpTestHeight = 2f;
    [SerializeField] private float jumpTestHeightProjectionDistance = 0.5f;
    //[SerializeField] private float fallTestHeight = 3f;
    //[SerializeField] private float fallTestHeightProjectionDistance = 0.5f;
    [SerializeField] private bool pingPongMode;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask wallLayers;
    [SerializeField] private float checkRadius;
    [SerializeField] private Vector2 CurrentDirection;

    [SerializeField]private float sightRange; 
    [SerializeField]private float attackRange;
    [SerializeField]private bool playerInSightRange; 
    [SerializeField]private bool playerInAttackRange;
    [SerializeField] private LayerMask playerLayers;
    [SerializeField] private float Speed = 1f;    
    
    private GameObject player;
    private Rigidbody2D rb;
    private float colliderHeight;

    private SpriteRenderer spriteRenderer;

    // Une liste contenant les checkpoints
    [SerializeField] private Transform[] listeDestinations;

    // Sens dans lequel on va passer d'un checkpoint a un autre (-1 pour aller a l'envers)
    private int sens = 1;

    // Le numero du checkPoint que l'on vise
    [SerializeField] private int indexDestination;

    // Distance a laquelle on considere qu'on a atteint un checkpoint
    [SerializeField] private float seuil = 0.1f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        colliderHeight = gameObject.GetComponent<Collider2D>().bounds.size.y;
    }

    private void Update()
    {
        //verifie la distance de vue et d'attaque
        Vector3 direction = player.transform.position - transform.position;
        playerInSightRange = Physics2D.Raycast(transform.position, direction, sightRange, playerLayers);
        playerInAttackRange = Physics2D.Raycast(transform.position, direction, attackRange, playerLayers);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }


    private void Patroling()
    {
        //on ne tire pas
        gameObject.GetComponent<Shooter>().enabled = false;

        // Une variable a laquelle j'assigne le Transform de notre checkpoint de destination
        //list qui contient tout les transform des point de patrol, et l'index permet d'en choisir 1
        Transform destination = listeDestinations[indexDestination];
        //distance = la distance entre le molotov et sont point de patrol
        float distance = transform.position.x - destination.position.x;

        //Si la distance est inférieure au seuil
        if (Mathf.Abs(distance) < seuil)
        {
            // On est assez proche du checkpoint pour considerer qu'on est arrive
            // On passe au checkpoint suivant en ajoutant 1 ou -1 en fonction du sens
            indexDestination += sens;

            // SI l'index de destination est superieur ou egale au nombre d'elements de la liste OU inferieur a 0
            if (indexDestination >= listeDestinations.Length || indexDestination < 0)
            {
                // Si on est en mode pingpong
                if (pingPongMode)
                {
                    SwapDirection();
                }
                else // SINON
                {
                    // On change l'index de destination pour revenir au premier element
                    indexDestination = 0;
                }
            }
        }
        else
        {
            if (distance > 0)
            {
                // Je dois aller a gauche
                CurrentDirection = Vector3.left;
                //spriteRenderer.flipX = true;
            }

            if (distance < 0)
            {
                // Je dois aller a droite
                CurrentDirection = Vector3.right;
                //spriteRenderer.flipX = false;
            }
            transform.Translate(CurrentDirection * Time.deltaTime * speed);

            /*RaycastHit2D hitVoid = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + CurrentDirection * fallTestHeightProjectionDistance, Vector2.down, fallTestHeight, groundLayers);
            if (hitVoid.transform == null)
            {
                SwapDirection();
            }*/
        }

        /*if (Physics2D.OverlapCircle(transform.position, checkRadius, groundLayers) != null)
        {
            
            RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down, colliderHeight/1.98f , groundLayers);
            RaycastHit2D hitWall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + jumpTestHeight), CurrentDirection, jumpTestHeightProjectionDistance, groundLayers);
            if (hitGround.transform != null)
            {
                if(hitWall.transform != null)
                {
                    // le mur est trop haut !
                    SwapDirection();
                }
                else
                {
                     rb.velocity = Vector2.up * jumpForce;
                }  
            }
        }*/
    }

    private void SwapDirection()
    {
        // On inverse le sens
        sens = -sens;
        // On rechange l'index de destination pour revenir dans la plage de la liste
        indexDestination += sens;
    }

        private void ChasePlayer()
    {
        //on ne tire pas
        gameObject.GetComponent<Shooter>().enabled = false;
        Vector3 direction;

        if (transform.position.x > player.transform.position.x)
        {
            direction= Vector3.left;
        }
        else
        {
            direction= Vector3.right;
        }

        transform.Translate(direction * Time.fixedDeltaTime * Speed);
        
        if (Physics2D.OverlapCircle(transform.position, checkRadius, wallLayers) != null) 
        {
        RaycastHit2D hitWall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + jumpTestHeight), direction, jumpTestHeightProjectionDistance, groundLayers);
                if(hitWall.transform != null)
                {
                    // le mur est trop haut !
                    SwapDirection();
                }
                else
                {
                     rb.velocity = Vector2.up * jumpForce;
                }  
        }
        
    }

    private void AttackPlayer()
    {
        gameObject.GetComponent<Shooter>().enabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
 
    }
}