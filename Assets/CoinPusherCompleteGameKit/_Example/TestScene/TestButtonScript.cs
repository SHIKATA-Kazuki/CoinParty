using UnityEngine;
using System.Collections;

public class TestButtonScript : MonoBehaviour {


    public Vector3 spawnPos;
    public GameObject coinBigToken;
    public GameObject coinDoubleValue;
    public GameObject coinDoubleXP;
    public GameObject coinExpand;
    public GameObject coinJoker;
    public GameObject coinPrize;
    public GameObject coinShower;
    public GameObject coinWall;
    public GameObject gameController;
    private GameManagerScript gmScript;

    void Awake()
    {
        gmScript = gameController.GetComponent<GameManagerScript>();
    }
    private void SpawnTestCoin(GameObject coinGo)
    {
        Instantiate(coinGo, spawnPos, coinGo.transform.rotation);
    }

    public void coinBigTokenBtn()
    {
        SpawnTestCoin(coinBigToken);
    }
    public void coinDoubleValueBtn()
    {
        SpawnTestCoin(coinDoubleValue);
    }
    public void coinDoubleXPBtn()
    {
        SpawnTestCoin(coinDoubleXP);
    }
    public void coinExpandBtn()
    {
        SpawnTestCoin(coinExpand);
    }
    public void coinJokerBtn()
    {
        SpawnTestCoin(coinJoker);
    }
    public void coinPrizeBtn()
    {
        SpawnTestCoin(coinPrize);
    }
    public void coinShowerBtn()
    {
        SpawnTestCoin(coinShower);
    }
    public void coinWallBtn()
    {
        SpawnTestCoin(coinWall);
    }
    public void RandomPrize()
    {
        gmScript.SpawnOnePrize();
    }
}
