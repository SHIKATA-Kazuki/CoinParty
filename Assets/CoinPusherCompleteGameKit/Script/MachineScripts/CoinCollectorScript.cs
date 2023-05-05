using UnityEngine;
using System.Collections;

public class CoinCollectorScript : MonoBehaviour {

    private GameObject thisCollector;
    private GameManagerScript gameManagerScript;

    void Awake()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerScript>();
        thisCollector = this.gameObject;
        thisCollector.tag = "CoinCollector";
        Collider col = thisCollector.GetComponent<Collider>(); // the CoinCollector msut have a Collider!!
        if (col != null)
        {
            col.isTrigger = false;
        }
        else
        {
            col = thisCollector.AddComponent<MeshCollider>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        CoinStandardScript _cSS = collision.gameObject.GetComponent<CoinStandardScript>();
        if(_cSS != null)
        {
            int coinWin = _cSS.GetPointValue();
            gameManagerScript.SetPlayerActualCoins(coinWin);
            int xpWin = _cSS.GetxpValue();
            gameManagerScript.SetplayerXP(xpWin);
            gameManagerScript.CreateEffect(_cSS.collectEffect);         
        }
        PrizeBasicScript _pBS = collision.gameObject.GetComponent<PrizeBasicScript>();
        if (_pBS != null)
        {
            int coinWin = _pBS.GetPointValue();
            gameManagerScript.SetPlayerActualCoins(coinWin);
            int xpWin = _pBS.GetxpValue();
            gameManagerScript.SetplayerXP(xpWin);
            gameManagerScript.CreateEffect(_pBS.collectEffect);
        }

        //<<<<<<<<<<<<<<< Add here your specific coin script >>>>>>>>>>>>>>>>>>>

        if (collision.gameObject.tag == "CoinWall")
        {
            gameManagerScript.MoveWalls();
        }
        if (collision.gameObject.tag == "CoinPrize")
        {
            gameManagerScript.SpawnOnePrize();
        }
        if (collision.gameObject.tag == "CoinJoker")
        {
            gameManagerScript.TransformXCoins();
        }
        if (collision.gameObject.tag == "CoinBigToken")
        {
            gameManagerScript.CreateTheBigCoin();
        }
        if (collision.gameObject.tag == "CoinDoubleXp")
        {
            gameManagerScript.DoublepXPTime();
        }
        if (collision.gameObject.tag == "CoinDoubleValue")
        {
            gameManagerScript.DoublepValueTime();
        }
        if (collision.gameObject.tag == "CoinShower")
        {
            gameManagerScript.CoinShower();
        }
        if (collision.gameObject.tag == "CoinExpand")
        {
            gameManagerScript.CoinExpander();
        }
    }

}
