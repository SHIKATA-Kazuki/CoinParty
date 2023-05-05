using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// a simple and not perfect Save system.
// 
public class SaveManagerScript : MonoBehaviour {

    private GameManagerScript gameManagerScript;

    void Awake()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerScript>();
    }

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< AutoSave Methodes >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

   // void OnApplicationFocus(bool focusStatus) // focusStatus = false if App is in background;
   public void SaveGTest() { 
   // {
        //if (!focusStatus)
        //{
            SaveAllData();
        //}
    }
    /*void OnApplicationPause(bool pauseStatus) // pauseStatus = true if App is paused;
    {
        if (pauseStatus)
        {
            SaveAllData();
        }
    }*/

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Public Methodes >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    public void SaveAllData()
    {
        SaveAllGameObjectsInAFile();
        SaveAllPlayerPrefs();
    }
    public void LoadAllCoinsAndPrizesOnGround()
    {
        // you can add other thing to load here (like some PlayersPrefs, or anything)
        LoadFile();
    }

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< private Methodes >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    void SaveAllPlayerPrefs()
    {   
        PlayerPrefs.SetString("LastDateTimeForRegenOffline", DateTime.Now.ToBinary().ToString());
        PlayerPrefs.SetString("VungleIsInit", "False");
    }

    void SaveAllGameObjectsInAFile() //Save all gameobjects and their position in current scene; save playerCoin and playerXP
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameSave.dat");

        GameObject[] goCoins = gameManagerScript.FindAllCoinsOnTheGround();
        GameObject[] goPrizes = gameManagerScript.FindAllPrizesOnTheGround();
        string[] _goCointag = new string[goCoins.Length]; //we save gameobject tag
        string[] _goCoinpos = new string[goCoins.Length]; // we save gameobject position in a string.
        string[] _goCoinRot = new string[goCoins.Length]; // we save gameobject rotation in a string.
        string[] _goPrizetag = new string[goPrizes.Length]; //we save gameobject tag
        string[] _goPrizepos = new string[goPrizes.Length]; // we save gameobject position in a string.
        string[] _goPrizerot = new string[goPrizes.Length]; // we save gameobject rotation in a string.
        if (goCoins.Length > 0)
        {
            for(int i=0; i < goCoins.Length; i++)
            {
                _goCointag[i] = goCoins[i].tag;
                _goCoinpos[i] = Vector3ToString(goCoins[i].transform.position);
                _goCoinRot[i] = Vector3ToString(goCoins[i].transform.eulerAngles);
            }
        }
        if (goPrizes.Length > 0)
        {
            for (int i = 0; i < goPrizes.Length; i++)
            {
                _goPrizetag[i] = goPrizes[i].tag;
                _goPrizepos[i] = Vector3ToString(goPrizes[i].transform.position);
                _goPrizerot[i] = Vector3ToString(goPrizes[i].transform.eulerAngles);
            }
        }
        GameData data = new GameData();
        data.coin = gameManagerScript.GetPlayerActualCoins();
        data.xp = gameManagerScript.GetplayerXP();
        data.goCoinTag = _goCointag;
        data.goCoinPos = _goCoinpos;
        data.goCoinRot = _goCoinRot;
        data.goPrizeTag = _goPrizetag;
        data.goPrizePos = _goPrizepos;
        data.goPrizeRot = _goPrizerot;

        bf.Serialize(file, data);
        file.Close();
    }
    void LoadFile() // load Coins and Prize on the ground, load playerCoin, playerXP
    {
        if (File.Exists(Application.persistentDataPath + "/gameSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameSave.dat", FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            //load correct values.
            gameManagerScript.SetPlayerActualCoins(data.coin, true);
            gameManagerScript.SetplayerXp(data.xp, true);
            CreateCoins(data.goCoinTag, data.goCoinPos, data.goCoinRot);
            CreatePrizes(data.goPrizeTag, data.goPrizePos, data.goPrizeRot);

        }
    }
    private void CreateCoins(string[] gocointag, string[] gocoinpos, string[] gocoinrot)
    {
        for(int i = 0; i < gocointag.Length; i++)
        {

            GameObject go = Instantiate(GetCoinGameobjectFromTag(gocointag[i]), Vector3FromString(gocoinpos[i]), Quaternion.Euler( Vector3FromString(gocoinrot[i]))) as GameObject;
        }
    }
    private void CreatePrizes(string[] goprizetag, string[] goprizepos, string[] goprizerot)
    {
        for (int i = 0; i < goprizetag.Length; i++)
        {
            Instantiate(GetPrizeGameobjectFromTag(goprizetag[i]), Vector3FromString(goprizepos[i]), Quaternion.Euler(Vector3FromString(goprizerot[i])));
        }
    }
    private GameObject GetCoinGameobjectFromTag(string tag)
    {
        int index = 0;
        while (gameManagerScript.coinManagerScript.coinS[index].coinGo.tag != tag)
        {
            index++;
        }
        return gameManagerScript.coinManagerScript.coinS[index].coinGo;
    }
    private GameObject GetPrizeGameobjectFromTag(string tag)
    {
        int index = 0;
        while (gameManagerScript.prizeManagerScript.prizeS[index].prizeGo.tag != tag)
        {
            index++;
        }
        return gameManagerScript.prizeManagerScript.prizeS[index].prizeGo;

    }
    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Class >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


    [Serializable]
    class GameData
    {
        public int coin;
        public int xp;
        public string[] goCoinTag;
        public string[] goCoinPos;
        public string[] goCoinRot;
        public string[] goPrizeTag;
        public string[] goPrizePos;
        public string[] goPrizeRot;
    }



    //Helpers
    private static string Vector3ToString(Vector3 v)
    { // change 0.000 to 0.0000 or any other precision you desire, i am saving space by using only 3 digits
        return string.Format("{0:0.000},{1:0.000},{2:0.000}", v.x, v.y, v.z);
    }

    private static Vector3 Vector3FromString(String s)
    {
        string[] parts = s.Split(new string[] { "," }, StringSplitOptions.None);
        return new Vector3(
            float.Parse(parts[0]),
            float.Parse(parts[1]),
            float.Parse(parts[2]));
    }

}
