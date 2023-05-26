using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float jumpTestHeight = 2f;
    [SerializeField] private float jumpTestHeightProjectionDistance = 0.5f;
    [SerializeField] private float fallTestHeight = 3f;
    [SerializeField] private float fallTestHeightProjectionDistance = 0.5f;
    [SerializeField] private bool pingPongMode;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float checkRadius;
    [SerializeField] private Vector2 CurrentDirection;
    private Rigidbody2D rb;
    private float colliderHeight;

    // Une liste contenant les checkpoints
    [SerializeField] private Transform[] listeDestinations;

    // Sens dans lequel on va passer d'un checkpoint � un autre (-1 pour aller � l'envers)
    private int sens = 1;

    // Le num�ro du checkPoint que l'on vise
    private int indexDestination;

    // Distance � laquelle on consid�re qu'on a atteint un checkpoint
    [SerializeField] private float seuil = 0.1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        colliderHeight = gameObject.GetComponent<Collider2D>().bounds.size.y;
    }

    void FixedUpdate()
    {
        // Une variable � laquelle j'assigne le Transform de notre checkpoint de destination
        Transform destination = listeDestinations[indexDestination];
        float distance = transform.position.x - destination.position.x;

        if (Mathf.Abs(distance)<seuil)
        {
            // On est assez proche du checkpoint pour considerer qu'on est arriv�

            // On passe au checkpoint suivant en ajoutant 1 ou -1 en fonction du sens
            indexDestination += sens;

            // SI l'index de destination est superieur ou �gale au nombre d'elements de la liste OU inferieur � 0
            if (indexDestination >= listeDestinations.Length || indexDestination < 0)
            {
                // Si on est en mode pingpong
                if (pingPongMode)
                {
                    SwapDirection();
                }
                else // SINON
                {
                    // On change l'index de destination pour revenir au premier �l�ment
                    indexDestination = 0;
                }
            }
        }
        else
        {
            if (distance > 0)
            {
                // Je dois aller � gauche
                CurrentDirection = Vector3.left;
            }

            if (distance < 0)
            {
                // Je dois aller � droite
                CurrentDirection = Vector3.right;
            }
            transform.Translate(CurrentDirection * Time.deltaTime * speed);

            RaycastHit2D hitVoid = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + CurrentDirection * fallTestHeightProjectionDistance, Vector2.down, fallTestHeight, groundLayers);
            if (hitVoid.transform == null)
            {
                SwapDirection();
            }
        }

        if (Physics2D.OverlapCircle(transform.position, checkRadius, groundLayers) != null) {
            
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
        }
    }
        
    private void SwapDirection()
    {
        // On inverse le sens
        sens = -sens;
        // On rechange l'index de destination pour revenir dans la plage de la liste
        indexDestination += sens;
    }
}
