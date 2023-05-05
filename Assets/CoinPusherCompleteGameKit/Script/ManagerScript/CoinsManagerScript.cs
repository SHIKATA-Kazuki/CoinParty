using UnityEngine;
using System.Collections;


[System.Serializable]
public class Coin
{
    public string coinName; // only needed to editor view
    public GameObject coinGo; 
    public float coinWeight; // the drop rate of the coin

    public Coin(string cName, GameObject cGameObject, float cWeight)
    {
        coinName = cName;
        coinGo = cGameObject;
        coinWeight = cWeight;
    }
}

public class CoinsManagerScript : MonoBehaviour {

    public Coin[] coinS;
    private float [] coinProbaTab;

    void Awake()
    {
        CreateProbalitiesTab(); 
    }

    public GameObject GetOneCoin() // return a random coin
    {
        float rand = Random.Range(0, coinProbaTab[coinProbaTab.Length - 1]);
        int i = 0;
        while (rand >= coinProbaTab[i])
        {
            i++;
        }
        return coinS[i].coinGo;  
    }
    public GameObject GetOneSpecialCoin() // return only a specialcoin.
    {
        int index= 0;
        bool boo = true;
        while(boo){
            index = Random.Range(0, coinS.Length);
            if(coinS[index].coinGo.tag != "CoinStandard")
            {
                boo = false;
            }
        }  
        return coinS[index].coinGo;
    }

    private void CreateProbalitiesTab() // create a new tab with the cumulates probabilities
    {
        float prevI = 0;
        coinProbaTab = new float[coinS.Length];
        for (int i=0; i< coinS.Length; i++)
        {            
            coinProbaTab[i] = coinS[i].coinWeight+prevI;
            prevI = coinProbaTab[i];
        }
    }


}
