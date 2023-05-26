using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private float TimeSinceLastShot = 0f;
    [SerializeField] private float ShootingInterval = 4f;
    [SerializeField] private float Range = 5f;
    [SerializeField] private float ProjectileSpeed = 10f;
    [SerializeField] private GameObject Projectile;
    [SerializeField] private LayerMask groundLayers;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("PlayerCible");
    }

    // Update is called once per frame
    void Update()
    {
        TimeSinceLastShot+=Time.deltaTime;

        float distance = Vector2.Distance(transform.position, Player.transform.position);
        if (distance <= Range)
        {//Nous sommme a distance de tir !
            Vector2 direction = Player.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, groundLayers);
            if (hit.transform==null)
            {//Je vois le player !
                print("vu");
                if (TimeSinceLastShot >= ShootingInterval)
                {
                    TimeSinceLastShot = 0;
                    float angle = Vector2.SignedAngle(Vector2.right, direction.normalized);
                    GameObject projectile = Instantiate(Projectile,transform.position, Quaternion.Euler(0, 0, angle));
                    projectile.GetComponent<Projectile>().SetSpeed(ProjectileSpeed);
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        if (Player != null)
        {
            Gizmos.color = Color.red;
            Vector2 direction = Player.transform.position - transform.position;
            Gizmos.DrawRay(transform.position, direction.normalized * Range);
        }
    }
}
