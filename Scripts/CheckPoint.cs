using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private LifeAndDeath lifeAndDeath;
    [SerializeField] public Transform updateCheckPointPosition;
    [SerializeField] public bool newCheckPoint = false;
    [SerializeField] private int numbercollision = 1;
    public Animator animator;



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            lifeAndDeath.lastCheckPointPos.transform.position = transform.position;
            if (numbercollision == 1)
            {
                newCheckPoint = true;
                lifeAndDeath.isRegen = newCheckPoint;
                numbercollision += 1;
                
                animator.SetTrigger("Rising");
            }

        }
    }
}
