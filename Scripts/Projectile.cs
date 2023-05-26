using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float TimeBeforeSelfDestruct = 4f;
    [SerializeField] private bool isMolotovFire = false;
    [SerializeField] private bool isMolotovIce = false;




    void Start()
    {
       Destroy(gameObject, TimeBeforeSelfDestruct);
    }

    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Explosion explosion) || other.TryGetComponent(out PlayerMove playerMove))
        {
            if (isMolotovFire == true)
            {
                //other.gameObject.SendMessage("Detonator", SendMessageOptions.DontRequireReceiver);
                explosion.hit = true;
            }

            if (isMolotovIce == true)
            {
                other.gameObject.SendMessage("SlowWalk", SendMessageOptions.DontRequireReceiver);
                //playerMove.SlowWalk();
            }
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }


}
