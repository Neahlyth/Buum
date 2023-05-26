using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyWalk : MonoBehaviour
{
    private GameObject Player;
    [SerializeField] private float SightRange = 5f;
    [SerializeField] private LayerMask TestLayers;
    [SerializeField] private float Speed = 1f;
    [SerializeField] private float AttackRange = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, Player.transform.position);
        
        if (distance <= AttackRange)
        {
            print("Attaque !");
        }
        else
        {
            if (IsOnSight(Player, SightRange, TestLayers))
            {
                FollowPlayer();
            }
        }
    }

    void FollowPlayer()
    {
        Vector3 direction;

        if (transform.position.x > Player.transform.position.x)
        {
            direction= Vector3.left;
        }
        else
        {
            direction= Vector3.right;
        }

        transform.Translate(direction * Time.fixedDeltaTime * Speed);

        // Le code qui remplace les lignes precedentes pour une version volante de l'enemi

        // Vector3 direction = Player.transform.position - transform.position;
        // transform.Translate(direction.normalized * Time.fixedDeltaTime * Speed);
    }

    bool IsOnSight(GameObject target,float range,LayerMask targetLayer)
    {
        float distance = Vector2.Distance(transform.position, target.transform.position);
        if (distance <= range)
        {
            print("vu");
            Vector3 direction = target.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, targetLayer);
            if (hit.transform == target.transform)
            {
                print("test");
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
