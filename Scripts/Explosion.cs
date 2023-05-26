using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;


public class Explosion : MonoBehaviour
{
    [SerializeField] private float radius;
    private bool detonator;
    private bool isExploding;
    public bool hit;
    [SerializeField] LayerMask explodedWallLayer;
    public static bool isActive;
    [SerializeField] private GameObject windowWarningMessageExplosion;


    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        windowWarningMessageExplosion.SetActive(isActive);
    }

    // Update is called once per frame
    void Update()
    {
        detonator = Input.GetKeyDown("x");
        Detonator();
        animator.SetBool("Exploding", isExploding);
        FirstExplosion();

    }


    public void Detonator()
    {
            
        if (detonator)
        {
            isExploding = true;

            Collider2D[] tabEnemie = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D enemie in tabEnemie)
            {
                //enemie.SendMessage("Die"/*, SendMessageOptions.DontRequireReceiver*/);
                if(enemie.TryGetComponent(out DieAndDestruct dieAndDestruct))
                {
                    dieAndDestruct.Die();
                }
            }
        }
        else if (hit)
        {
            isExploding = true;
            hit = false;
        }
        else
        {
            isExploding=false;
        }
    }

    private void FirstExplosion ()
    {
        Collider2D wall = Physics2D.OverlapCircle(transform.position, radius, explodedWallLayer);
        if (wall != null)
        {
            if (wall.gameObject.TryGetComponent(out DieAndDestruct dieAndDestruct))
            {
                isActive= true;
                //Debug.Log(1);
            }
            else
            {
                isActive=false;
                //Debug.Log(2);
            }
        }
        windowWarningMessageExplosion.SetActive(isActive);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
