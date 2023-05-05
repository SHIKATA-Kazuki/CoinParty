using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class TapHereScript : MonoBehaviour {

    private GameObject thisTapHere;
    private CoinsManagerScript coinManagerScript;
    private GameManagerScript gameManagerScript;
    private float spawnHight=2;
    private bool canTap = true;
    private float tapActionRegenTime;
    private int tapActionLimit;
    private int tapActionActualNumber;
    private float tapActionLeft;
    private bool coroutineRegenIsStart=false;
    private AudioSource _audio;
    private AudioClip tapEmptySound;

    void Awake()
    {        
        thisTapHere = this.gameObject;
        thisTapHere.layer = 8;// layer 8 = "TapHereGO" // the "TapHereZone" don't collid with other gameobject.
        Physics.IgnoreLayerCollision(8, 0, true);
        Physics.IgnoreLayerCollision(8, 2, true);
        Collider col = thisTapHere.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false;
        }
        else
        {
            col = thisTapHere.AddComponent<MeshCollider>();
        }
        GameObject _gameController = GameObject.FindGameObjectWithTag("GameController");
        coinManagerScript = _gameController.GetComponent<CoinsManagerScript>();
        gameManagerScript = _gameController.GetComponent<GameManagerScript>();
        _audio = thisTapHere.GetComponent<AudioSource>();
        _audio.playOnAwake = false;
        tapEmptySound = gameManagerScript.tapEmptySound;
        tapActionRegenTime = gameManagerScript.tapActionRegenTime;
        tapActionLimit = gameManagerScript.tapActionLimit;
        tapActionLeft = tapActionLimit;
    }

    void Update()
    {        
        //check if player has at least One coin
          
        if (Input.GetMouseButtonDown(0) )
        {
            //instantiate coin when raycast hit "TapHere"
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.collider.gameObject == thisTapHere && gameManagerScript.GetPlayerActualCoins() > 0 && canTap)
                {
                    if(gameManagerScript.GetPlayerActualCoins() > 0 && canTap)
                    {
                        Vector3 spawnPosition = hit.point + new Vector3(0, spawnHight, 0);
                        GameObject coinToInstantiate = coinManagerScript.GetOneCoin();
                        Instantiate(coinToInstantiate, spawnPosition, coinToInstantiate.transform.rotation);
                        gameManagerScript.SetPlayerActualCoins(-1);
                        tapActionLeft--;
                    } else
                    {
                        if(tapEmptySound != null)
                        {
                            _audio.clip = tapEmptySound;
                            _audio.Play();
                        }
                    }

                    
                }                
            }
        }
        if (canTap)
        {
            if(tapActionLeft == 0)
            {
                canTap = false;
            }            
        }
        if(tapActionLeft < tapActionLimit)
        {
            if (!coroutineRegenIsStart)
            {
                StartCoroutine("RegenTapAction");
                coroutineRegenIsStart = true;
            }
        }
    }
    IEnumerator RegenTapAction()
    {
        yield return new WaitForSeconds(tapActionRegenTime);
        tapActionLeft++;
        //if you want a display of action left, you can update this display here.
        // ex: create an array of your sprites (number of sprite must be the same as tapActionLimit)
        // 
        // mySpriteArray[tapActionLeft-1].setActive = true;
        //
        //
        canTap = true;
        coroutineRegenIsStart = false;
        yield return null;
    }

}
