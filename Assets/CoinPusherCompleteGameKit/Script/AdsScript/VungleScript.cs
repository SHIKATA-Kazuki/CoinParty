using UnityEngine;
using System.Collections;
using VungleSDKProxy;

public class VungleScript : MonoBehaviour {
    public string AndroidVungleID;
    public string IosVungleID;
    public int coinToAddIfComplet;
    
    private GameManagerScript gameManagerScript;

    // Use this for initialization
    void Start () {
        if(PlayerPrefs.GetString("VungleIsInit")!= "True")
        {
            Vungle.init(AndroidVungleID, IosVungleID, "Test_Windows");
            PlayerPrefs.SetString("VungleIsInit", "True");
        }
        if(GameObject.FindGameObjectWithTag("GameController") != null)
        {
            gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerScript>();
        }      

    }
	

    public void PlayVungleAd()
    {
        Vungle.adPlayableEvent += (isAdAvailable) => {
            if (isAdAvailable)
            {
                Vungle.playAd();
            }
        };
        Vungle.onAdFinishedEvent += (onAdFinishedEvent) =>
         {
             if (onAdFinishedEvent.IsCompletedView)
             {
                 if(gameManagerScript != null)
                 {
                     gameManagerScript.SetPlayerActualCoins(coinToAddIfComplet);
                 }
             }
         };
    }
}
