using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageEnd : MonoBehaviour
{
    [SerializeField] private GameObject endMessage;
    // Start is called before the first frame update
    void Start()
    {
        endMessage.SetActive(false);

    }


     void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.CompareTag ("Player"))
        {
            endMessage.SetActive(true);
        }
    }
}
