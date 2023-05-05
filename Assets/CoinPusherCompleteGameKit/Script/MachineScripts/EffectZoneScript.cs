using UnityEngine;
using System.Collections;

public class EffectZoneScript : MonoBehaviour {

    void OnDrawGizmos()
    {
        Vector3 effectZoneBounds = GetComponentInParent<Collider>().bounds.size;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, effectZoneBounds);
    }
    void Awake()
    {
        this.gameObject.layer = 2; // ignore Raycast
    }

}
