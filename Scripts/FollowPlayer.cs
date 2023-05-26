using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float smoothing;
    public Transform killZone;
    public float offsetKillZone;
    private Vector3 m_Velocity = Vector3.zero;

    // Update is called once per frame
    void LateUpdate()
    {
        if(player!=null)
        {
            float positionY = Mathf.Clamp(player.transform.position.y, killZone.position.y + offsetKillZone, float.PositiveInfinity);
            Vector3 targetPosition = new Vector3(player.transform.position.x, positionY, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_Velocity, smoothing);
        }
    }
}
