using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private int doorId;
    [SerializeField] private float speed;


    public void OpenDoors(int activePlateformId)
    {
        if (activePlateformId == doorId)
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed);

            
        }
    }
}
