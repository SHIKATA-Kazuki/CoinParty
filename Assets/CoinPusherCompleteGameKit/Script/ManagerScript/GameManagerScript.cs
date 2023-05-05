using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameManagerScript : MonoBehaviour {

    //Machine
    public GameObject ground;
    public GameObject walls;
    public GameObject pusher;
    public GameObject tapHereZone;
    public GameObject coinCollector;
    public GameObject destroyer;

    //Physics
    public float groundFriction=0.05f; // 0 to 1
    public float groundBounciness = 0; //0 to 1
    public float spawnHeightFromGround=2; //distance between ground and the spawn point.
    public float pusherSpeed=1; //pusher movement speed
    public float wallMoveHeight=4.0f; // distance between wall down and wall up
    public float wallSpeed = 1; // wall movement speed    

    //Rules
    //TapHereScript Rules
    public float tapActionRegenTime=1; // the regen time for One tapActionLimit
    public int tapActionLimit=4; // how many time the player can tap very quickly
    //Regen
    public int regenOfflineSpeed; // how many seconds to get One coin when when app not running, in seconds
    public int regenOfflineMaxCoin; //maximum number of coins that will be regenerate during offline
    public int regenOnlineSpeed; // how many seconds to get One coin when app is running, in seconds
    public int regenOnlineMaxCoin; //maximum number of coins that will be regenerate during online

    public float wallDuration = 5;
    //SpecialCoin
    public int jokerCoinNumber = 4; //how many coin will be transform
    public GameObject bigCoinGo;
    public float bigCoinPower;
    public float bigCoinRadius;
    private int XPMultipactor = 1;
    public float multiplXpDuration = 10;
    private int valueMultipactor = 1;
    public float multiplValueDuration = 10;
    public float expanderMultiplicator = 2;
    public int showerCoinNumber = 10;
    public float showerCoinDelay = 0.2f;

    //Player
    public bool useSave;
    public int startingCoin; // how many coins at the beginning
    public bool manageXP=false;
    private int playerActualCoins; 
    private int playerXP;
    public int[] XpPerLevel;
    private int[] levelCapTab; // the cumulate XP

    //UI
    public Text coinText; // Only "coinText" is Required.
    public Text xpTotalToNextLevelText;
    private int xpTotalToNextLevel;
    public Text xpCurrentToNextLevelText;
    private int xpCurrentToNextLevel;
    public Slider xpSlider;
    public Text playerLevelText;
    private int playerLevel;
    public Text timerOnlineText;
    public GameObject effectZone;

    //Sounds
    public AudioClip wallUpSound;
    public AudioClip wallDownSound;
    public AudioClip tapEmptySound;

    //Others
    public PrizesManagerScript prizeManagerScript;
    public CoinsManagerScript coinManagerScript;
    public bool vungleIsInit;

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Getter and Setter >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    #region getterAndSetter
    public void SetPlayerActualCoins(int coinsToAdd)
    {
        playerActualCoins += coinsToAdd * valueMultipactor;
        coinText.text = "" + playerActualCoins;
    }
    public void SetPlayerActualCoins(int coinsToAdd, bool resetPreviousValue)
    {
        if (resetPreviousValue)
        {
            playerActualCoins = coinsToAdd;
        } else
        {
            playerActualCoins += coinsToAdd * valueMultipactor;
        }        
        coinText.text = "" + playerActualCoins;
    }
    public int GetPlayerActualCoins()
    {
        return playerActualCoins;
    }
    public void SetplayerXP(int xPToAdd)
    {
        playerXP += xPToAdd* XPMultipactor;
        CheckLevel();
        UpdateXPUI();
    }
    public void SetplayerXp(int xpToAdd, bool resetPreviousXp) //if true : xpToAdd will be Total playerXp
    {
        if (resetPreviousXp)
        {
            playerXP = xpToAdd;
        } else
        {
            playerXP += xpToAdd* XPMultipactor;
        }
        CheckLevel();
        UpdateXPUI();

    }
    public int GetplayerXP()
    {
        return playerXP;
    }
   
    
    #endregion

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Starting Methodes >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    #region startingMethodes
    void Awake()
    {
        CheckTag();    
        prizeManagerScript = this.gameObject.GetComponent<PrizesManagerScript>();
        coinManagerScript = this.gameObject.GetComponent<CoinsManagerScript>();
        pusher.GetComponent<PusherScript>().InitPusherData();
        SetPusherSpeed();
        SetGroundCaract();
        CreateLevelCapTab();
    }

    void Start()
    {
        if (!useSave)
        {
            PlayerPrefs.SetInt("firstPlay", 0);
        }
        if (PlayerPrefs.GetInt("firstPlay") == 0)
        {
            PlayerPrefs.SetInt("firstPlay", 1);
            playerActualCoins = startingCoin;
        }
        else
        {
            GameObject[] goS = FindAllCoinsAndPrizesOnTheGround();
            foreach (GameObject go in goS)
            {
                Destroy(go);
            }
            SaveManagerScript _saveManagerScript = this.gameObject.GetComponent<SaveManagerScript>();
            _saveManagerScript.LoadAllCoinsAndPrizesOnGround();
            if (playerActualCoins < regenOfflineMaxCoin) // Only if needed, we add coins win durind offline.
            {
                int coinToAdd = this.gameObject.GetComponent<RegenManagerScript>().GetTotalCoinRegenOffline();
                playerActualCoins += coinToAdd;
                if (playerActualCoins > regenOfflineMaxCoin)
                {
                    playerActualCoins = regenOfflineMaxCoin;
                }
            }
        }
        coinText.text = ""+playerActualCoins;
        CheckLevel();
        UpdateXPUI();
    }

    #endregion

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Level XP Methodes>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    #region levelXpMethodes

    void CreateLevelCapTab() // create a new array with the cumulate XP, call on awake
    {
        levelCapTab = new int[XpPerLevel.Length];
        for (int i = 0; i < levelCapTab.Length; i++)
        {
            if (i == 0)
            {
                levelCapTab[i] = XpPerLevel[i];
            } else
            {
                levelCapTab[i] = levelCapTab[i - 1] + XpPerLevel[i];
            }            
        }
    }

    public void CheckLevel() // check player level
    {
        int oldPlayerLevel = playerLevel;
        if (manageXP)
        {
            if (playerXP >= levelCapTab[levelCapTab.Length - 1])
            {
                playerLevel = levelCapTab.Length;
                xpCurrentToNextLevel = XpPerLevel[XpPerLevel.Length - 1];
                xpTotalToNextLevel = XpPerLevel[XpPerLevel.Length - 1];
            }
            else
            {
                int i = 0;
                while (playerXP >= levelCapTab[i] && i < levelCapTab.Length - 1)
                {
                    i++;
                }
                playerLevel = i;
                xpCurrentToNextLevel = playerXP - levelCapTab[i - 1];
                xpTotalToNextLevel = levelCapTab[playerLevel] - levelCapTab[playerLevel - 1];
            }
        }
        if(oldPlayerLevel != playerLevel)
        {
            PlayerLevelUp();
        }

    }
    void PlayerLevelUp()
    {
        //do what you want when player levelup (create a special coin, spawn a prize, ...) or nothing ))
    }

    void UpdateXPUI() //Update the "non-required" interface
    {
        if (manageXP)
        {
            if (playerLevelText != null)
                playerLevelText.text = "" + playerLevel;
            if (xpTotalToNextLevelText != null)
                xpTotalToNextLevelText.text = "" + xpTotalToNextLevel;
            if (xpCurrentToNextLevelText != null)
                xpCurrentToNextLevelText.text = "" + xpCurrentToNextLevel;
            if (xpSlider != null)
                xpSlider.value = (float)xpCurrentToNextLevel / (float)xpTotalToNextLevel;
        }
    }


    #endregion

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Machine Physic Methodes >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    #region MachinePhysic

    private void SetGroundCaract() //Call on Awake
    {
        PhysicMaterial material = new PhysicMaterial();
        material.dynamicFriction = groundFriction;
        material.staticFriction = 0;
        material.frictionCombine = PhysicMaterialCombine.Minimum;
        material.bounciness = groundBounciness;
        material.bounceCombine = PhysicMaterialCombine.Minimum;
        ground.GetComponent<Collider>().material = material;
        pusher.GetComponent<Collider>().material = material;
    }
    private void SetPusherSpeed() //Call on Awake
    {
        pusher.GetComponent<PusherScript>().SetPusherSpeed(pusherSpeed);
    }

    public void CreateEffect(GameObject effectGO)
    {
        if (effectGO != null)
        {
            Vector3 posToInstantiate = RandPosInAGameObject(effectZone);
            Instantiate(effectGO, posToInstantiate, effectGO.transform.rotation);
        }
    }

    #endregion

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Special Coin Methodes >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Add here your specific coin script >>>>>>>>>>>>>>>>>>>>>>>>>
    #region SpecialCoin

    public void MoveWalls()
    {
        walls.GetComponent<WallsScript>().MoveWalls(wallDuration, wallSpeed);
    }
    public void SpawnOnePrize() // spawn a prize over "TapHereZone" minus by 10% to compensate the prize size.
    {
        GameObject prizeToSpawn = prizeManagerScript.GetOnePrize();
        Vector3 tapHereBounds = tapHereZone.gameObject.GetComponent<Collider>().bounds.size;
        float xMin = tapHereZone.transform.position.x - (tapHereBounds.x / 2) * 0.9f; 
        float xMax = tapHereZone.transform.position.x + (tapHereBounds.x / 2) * 0.9f;
        float zMin = tapHereZone.transform.position.z - (tapHereBounds.z / 2) * 0.9f;
        float zMax = tapHereZone.transform.position.z + (tapHereBounds.z / 2) * 0.9f;
        Instantiate(prizeToSpawn, new Vector3(Random.Range(xMin, xMax), tapHereZone.transform.position.y + spawnHeightFromGround*2, Random.Range(zMin, zMax)), prizeToSpawn.transform.rotation);
    }
    public void TransformXCoins() // Change X standards coins in X specials coins
    {
        GameObject[] AllStandardCoin = GameObject.FindGameObjectsWithTag("CoinStandard") as GameObject[];
        List<GameObject> AllStandardCoinList = new List<GameObject>();
        AllStandardCoinList = AllStandardCoin.ToList();
        for (int i = 0; i<= jokerCoinNumber; i++)
        {
            if (AllStandardCoinList.Count > 0)
            {
                int coinIndex = Random.Range(0, AllStandardCoinList.Count);
                Instantiate(coinManagerScript.GetOneSpecialCoin(), AllStandardCoinList[coinIndex].transform.position, AllStandardCoinList[coinIndex].transform.rotation);
                Destroy(AllStandardCoinList[coinIndex]);
                AllStandardCoinList.RemoveAt(coinIndex);
            }
        }
    }
    public void CreateTheBigCoin()
    {
        Instantiate(bigCoinGo, new Vector3(tapHereZone.transform.position.x, tapHereZone.transform.position.y + spawnHeightFromGround * 2, tapHereZone.transform.position.z), tapHereZone.transform.rotation);
    }

    public void DoublepXPTime()
    {
        StartCoroutine(MultiplXpTime(2));
    }
    IEnumerator MultiplXpTime(int multiplicator)
    {
        XPMultipactor = multiplicator;
        yield return new WaitForSeconds(multiplXpDuration);
        XPMultipactor = 1;
        yield return null;
    }

    public void DoublepValueTime()
    {
        StartCoroutine(MultiplValueTime(2));
    }
    IEnumerator MultiplValueTime(int multiplicator)
    {
        valueMultipactor = multiplicator;
        yield return new WaitForSeconds(multiplValueDuration);
        valueMultipactor = 1;
    }

    public void CoinShower()
    {
        StartCoroutine(CreateCoinShower());

    }
    IEnumerator CreateCoinShower()
    {
        for (int i = 0; i < showerCoinNumber; i++)
        {
            yield return new WaitForSeconds(showerCoinDelay);
            Vector3 posToInstantiate = RandPosInAGameObject(tapHereZone);
            posToInstantiate = new Vector3(posToInstantiate.x, posToInstantiate.y + 2, posToInstantiate.z);
            GameObject coinGo = coinManagerScript.GetOneCoin();
            Instantiate(coinGo, posToInstantiate, coinGo.transform.rotation);
        }
        yield return null;

    }
    public void CoinExpander()
    {
        GameObject[] coinsGo = FindAllCoinsOnTheGround();
        foreach (GameObject coin in coinsGo)
        {
            coin.transform.localScale = coin.transform.localScale* expanderMultiplicator;
        }
    }
    

    #endregion

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Helper Methodes >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    #region Helper

    public GameObject[] FindAllCoinsAndPrizesOnTheGround()
    {
        GameObject[] prizes = FindAllPrizesOnTheGround();
        GameObject[] coins = FindAllCoinsOnTheGround();
        List<GameObject> CoinAndPrizeList = new List<GameObject>();
        CoinAndPrizeList = coins.ToList();
        if (prizes.Length > 0)
        {
            for (int i = 0; i < prizes.Length; i++)
            {
                CoinAndPrizeList.Add(prizes[i]);
            }
        }
        return CoinAndPrizeList.ToArray();
    }
    public GameObject[] FindAllCoinsOnTheGround()
    {        
        string coinTag = "";
        List<GameObject> AllCoinOnGroundList = new List<GameObject>();
        if(coinManagerScript.coinS.Length > 0)
        {
            for (int i = 0; i < coinManagerScript.coinS.Length; i++)
            {
                coinTag = coinManagerScript.coinS[i].coinGo.tag;
                GameObject[] AllCoinsByTag = GameObject.FindGameObjectsWithTag(coinTag);
                if (AllCoinsByTag.Length > 0)
                {
                    for (int n = 0; n < AllCoinsByTag.Length; n++)
                    {
                        AllCoinOnGroundList.Add(AllCoinsByTag[n]);
                    }
                }
            }
        }
        return AllCoinOnGroundList.ToArray();
    }
    public GameObject[] FindAllPrizesOnTheGround()
    {
        string prizeTag = "";
        List<GameObject> AllPrizeOnGroundList = new List<GameObject>();
        if(prizeManagerScript.prizeS.Length > 0)
        {
            for (int i = 0; i < prizeManagerScript.prizeS.Length; i++)
            {
                prizeTag = prizeManagerScript.prizeS[i].prizeGo.tag;
                GameObject[] AllPrizesByTag = GameObject.FindGameObjectsWithTag(prizeTag);
                if (AllPrizesByTag.Length > 0)
                {
                    for (int n = 0; n < AllPrizesByTag.Length; n++)
                    {
                        AllPrizeOnGroundList.Add(AllPrizesByTag[n]);
                    }
                }
            }
        }
        return AllPrizeOnGroundList.ToArray();
    }

    public Vector3 RandPosInAGameObject(GameObject goZone)
    {
        Vector3 zoneBounds = goZone.GetComponentInParent<Collider>().bounds.size;
        float posX = Random.Range(goZone.transform.position.x - zoneBounds.x, goZone.transform.position.x + zoneBounds.x);
        float posY = Random.Range(goZone.transform.position.y - zoneBounds.y, goZone.transform.position.y + zoneBounds.y);
        float posZ = Random.Range(goZone.transform.position.z - zoneBounds.z, goZone.transform.position.z + zoneBounds.z);
        Vector3 point = new Vector3(posX, posY, posZ);
        return point;
    }
    public void SaveGame()
    {
        SaveManagerScript _saveManagerScript = this.gameObject.GetComponent<SaveManagerScript>();
        _saveManagerScript.SaveAllData();
    }
    public void LoadGame()
    {
        SaveManagerScript _saveManagerScript = this.gameObject.GetComponent<SaveManagerScript>();
        _saveManagerScript.LoadAllCoinsAndPrizesOnGround();
    }

    #endregion

    private void CheckTag()
    {

    }
}
