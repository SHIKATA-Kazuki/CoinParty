using UnityEngine;
using System.Collections;

public class PusherMaxDistScript : MonoBehaviour {
    

    void OnDrawGizmos()
    {
        Vector3 pusherBounds = GetComponentInParent<Collider>().bounds.size;    
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, pusherBounds);
    }
    void Awake()
    {
        this.gameObject.layer = 2; // ignore Raycast
    }
}
