using UnityEngine;
using System.Collections;

public class CoinDestroyerScript : MonoBehaviour {

    private GameObject thisDestroyer;

    void Awake()
    {
        thisDestroyer = this.gameObject;
        thisDestroyer.tag = "Destroyer";
        Collider col = thisDestroyer.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false;
        } else
        {
            col = thisDestroyer.AddComponent<MeshCollider>();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        CoinStandardScript _cSS = collision.gameObject.GetComponent<CoinStandardScript>();
        if(_cSS != null)
        {
            GameObject effect = _cSS.looseEffect;
            if (effect != null)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerScript>().CreateEffect(effect);
            }
        }
        PrizeBasicScript _pBS = collision.gameObject.GetComponent<PrizeBasicScript>();
        if (_pBS != null)
        {
            GameObject effect = _pBS.looseEffect;
            if (effect != null)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerScript>().CreateEffect(effect);
            }
        }
        Destroy(collision.gameObject);
    }

}
