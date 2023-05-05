using UnityEngine;
using System.Collections;

[System.Serializable]
public class Prize
{
    public string prizeName; //only needed to editor view
    public GameObject prizeGo;
    public float prizeWeight; // the drop rate of the prize

    public Prize(string pName, GameObject pGameObject, float pWeight)
    {
        prizeName = pName;
        prizeGo = pGameObject;
        prizeWeight = pWeight;
    }
}

public class PrizesManagerScript : MonoBehaviour {

    public Prize[] prizeS;
    private float[] prizeProbaTab;

    void Awake()
    {
        CreateProbalitiesTab();
    }

    public GameObject GetOnePrize() // return a random prize
    {
        float rand = Random.Range(0, prizeProbaTab[prizeProbaTab.Length - 1]);
        int i = 0;
        while (rand >= prizeProbaTab[i])
        {
            i++;
        }
        return prizeS[i].prizeGo;
    }

    private void CreateProbalitiesTab() // create a new tab with the cumulates probabilities
    {
        float prevI = 0;
        prizeProbaTab = new float[prizeS.Length];
        for (int i = 0; i < prizeS.Length; i++)
        {
            prizeProbaTab[i] = prizeS[i].prizeWeight + prevI;
            prevI = prizeProbaTab[i];
        }
    }

}
