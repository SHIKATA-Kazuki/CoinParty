using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class CoinStandardScript : MonoBehaviour {

    private GameObject thisCoin;
    private AudioSource _audio;
    public int pointValue;
    public int xPValue;
    private bool dropSoundMustPlay=true;
    public AudioClip dropSound;
    public AudioClip collectSound;
    public AudioClip looseSound;
    public GameObject dropEffect;
    public GameObject collectEffect;
    public GameObject looseEffect;

    public int GetPointValue() // how many new Coins will be given by the coin
    {
        return pointValue;            
    }
    public int GetxpValue() // how many Xp will be given by the coin
    {
        return xPValue;
    }
	
	void Awake () {
        thisCoin = this.gameObject;
        thisCoin.layer = 2; // ignore Raycast
        _audio = thisCoin.GetComponent<AudioSource>();
        _audio.playOnAwake = false;        
	    if (thisCoin.GetComponent<Collider>() == null)
        {
            MeshCollider col= thisCoin.AddComponent<MeshCollider>();
            col.convex = true;
        } else
        {
            thisCoin.GetComponent<Collider>().isTrigger = false; // CoinCollector and CoinDestroyer need that.
        }
        if (dropEffect != null) // instantiate particuleEffect and set it as child of the coin.
        {
            GameObject _dropEffect = Instantiate(dropEffect, thisCoin.transform.position, Quaternion.identity) as GameObject;
            _dropEffect.transform.parent = thisCoin.transform;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (dropSoundMustPlay && dropSound != null)
        {
            _audio.clip = dropSound;
            _audio.Play();
            dropSoundMustPlay = false;
        }
        if (collision.gameObject.tag == "CoinCollector")
        {
            _audio.clip = collectSound;
            if (collectSound != null)
            {
                _audio.Play();
                StartCoroutine(DestroyAfterXXseconds(_audio.clip.length));
            }
            else
            {
                Destroy(thisCoin);
            }

        }
        if (collision.gameObject.tag == "Destroyer")
        {
            _audio.clip = looseSound;
            if (looseSound != null)
            {
                _audio.Play();
                StartCoroutine(DestroyAfterXXseconds(_audio.clip.length));
            }
            else
            {
                Destroy(thisCoin);
            }
        }
    }

    IEnumerator DestroyAfterXXseconds(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(thisCoin);
        yield return null;
    }





}
