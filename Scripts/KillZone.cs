using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] private Transform killZone;
    [SerializeField] private float killZoneHeight;
    [SerializeField] private float killZoneWidth;

    void OnTriggerEnter2D(Collider2D col)
    {
        print("meurt");
        col.gameObject.SendMessage("DoDie", SendMessageOptions.DontRequireReceiver);
    }
    
    void OnDrawGizmos()
    {
        // Dessine un cube vert a la position du groundCheck
        Gizmos.color = new Color32(255, 0, 0, 90);
        Gizmos.DrawCube(killZone.position, new Vector2(killZoneWidth, killZoneHeight));
    }
}
