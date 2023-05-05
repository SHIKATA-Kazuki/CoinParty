using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class PrizeBasicScript : MonoBehaviour {

    private GameObject thisPrize;
    private AudioSource _audio;
    public int pointValue;
    public int xPValue;
    private bool dropSoundMustPlay = true;
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

    void Awake()
    {
        thisPrize = this.gameObject;
        thisPrize.layer = 2; // ignore Raycast
        _audio = thisPrize.GetComponent<AudioSource>();
        _audio.playOnAwake = false;
        if (thisPrize.GetComponent<Collider>() == null)
        {
            thisPrize.AddComponent<MeshCollider>();
        }
        else
        {
            thisPrize.GetComponent<Collider>().isTrigger = false; // CoinCollector and CoinDestroyer need that.
        }
        if (dropEffect != null) // instantiate particuleEffect and set it as child of the coin.
        {
            GameObject _dropEffect = Instantiate(dropEffect, thisPrize.transform.position, Quaternion.identity) as GameObject;
            _dropEffect.transform.parent = thisPrize.transform;
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
        if (collision.gameObject.tag == "CoinCollector" && collectSound != null)
        {
            _audio.clip = collectSound;
            _audio.Play();
            StartCoroutine(DestroyAfterXXseconds(_audio.clip.length));
        }
        if (collision.gameObject.tag == "Destroyer" && looseSound != null)
        {
            _audio.clip = looseSound;
            _audio.Play();
            StartCoroutine(DestroyAfterXXseconds(_audio.clip.length));
        }
    }

    IEnumerator DestroyAfterXXseconds(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(thisPrize);
        yield return null;
    }
}
