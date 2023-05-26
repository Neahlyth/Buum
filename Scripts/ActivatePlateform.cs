using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class ActivatePlateform : MonoBehaviour
{
    [SerializeField] private Doors doors;
    [SerializeField] private int activatePlateformId;
    [SerializeField] private bool colliderActivate;

    private void Awake()
    {
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (colliderActivate && (collision.transform.tag == "Player"))
        {
            doors.OpenDoors(activatePlateformId);
            colliderActivate = false;
        }
        
    }


}
