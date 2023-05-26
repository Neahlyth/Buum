using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAndDestruct : MonoBehaviour
{
    //public Collider2D colliderEnemie;

    // Start is called before the first frame update
    void Start()
    {
        //colliderEnemie = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
