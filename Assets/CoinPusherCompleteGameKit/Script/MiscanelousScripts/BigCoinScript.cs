using UnityEngine;
using System.Collections;

public class BigCoinScript : MonoBehaviour {

    private float radius;
    private float power;
    private bool mustExplose=true;
    private GameManagerScript gameManagerScript;

    void Awake()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerScript>();
        power = gameManagerScript.bigCoinPower;
        radius = gameManagerScript.bigCoinRadius;
    }
    void OnCollisionEnter(Collision collision)
    {
        if(mustExplose)
        {
            Explosion();
            mustExplose = false;           
        }
    }
    void Explosion()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }
    }
    
}
