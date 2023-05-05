using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class RegenManagerScript : MonoBehaviour {

    private int _regenOfflineSpeed; // how many seconds to get One coin when when app not running
    private int _regenOfflineMaxCoin; //maximum number of coins that will be regenerate during offline
    private int _regenOnlineSpeed; // how many seconds to get One coin when app is running
    private int _regenOnlineMaxCoin; //maximum number of coins that will be regenerate during online
    private int totalCoinRegenOffline;
    private Text _timerOnlineText;
    private int timeLeftOnline;
    private GameManagerScript gameManagerScript;
    private DateTime currentDate;
    private DateTime oldDate;

    void Awake()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerScript>();
        _regenOfflineSpeed = gameManagerScript.regenOfflineSpeed;
        _regenOfflineMaxCoin = gameManagerScript.regenOfflineMaxCoin;
        _regenOnlineMaxCoin = gameManagerScript.regenOnlineMaxCoin;
        _timerOnlineText = gameManagerScript.timerOnlineText;
        _regenOnlineSpeed = gameManagerScript.regenOnlineSpeed;
        timeLeftOnline = _regenOnlineSpeed;
    }
    void Start()
    {
        InvokeRepeating("RegenOnlineTimer", 1.0f, 1.0f);
        if (_timerOnlineText != null)
        {
            _timerOnlineText.text = "";
        }
    }

    public int GetTotalCoinRegenOffline() // return the coins get during Offline.
    {

        //Store the current time when it starts
        currentDate = System.DateTime.Now;

        //Grab the old time from the player prefs as a long
        long temp = Convert.ToInt64(PlayerPrefs.GetString("LastDateTimeForRegenOffline"));

        //Convert the old time from binary to a DataTime variable
        DateTime oldDate = DateTime.FromBinary(temp);

        //Use the Subtract method and store the result as a timespan variable and convert it in Second (Integer)
        TimeSpan difference = currentDate.Subtract(oldDate);
        double secondsInDouble = difference.TotalSeconds;
        int secondsInInt = Convert.ToInt32(secondsInDouble);
        float tmp1 = secondsInInt % _regenOfflineSpeed;
        totalCoinRegenOffline = Convert.ToInt32((secondsInInt - tmp1) / _regenOfflineSpeed);
        if (totalCoinRegenOffline > _regenOfflineMaxCoin)
        {
            totalCoinRegenOffline = _regenOfflineMaxCoin;
        }
        return totalCoinRegenOffline;

    }
    void RegenOnlineTimer() // regeneret a coin if needed
    {
        int actualCoins = gameManagerScript.GetPlayerActualCoins();
        if (actualCoins < _regenOnlineMaxCoin)
        {
            timeLeftOnline--;
            if (_timerOnlineText != null)
            {
                _timerOnlineText.text = ""+ timeLeftOnline;
            }
            if (timeLeftOnline == 0)
            {
                gameManagerScript.SetPlayerActualCoins(1);
                timeLeftOnline = _regenOnlineSpeed;
            }
        } else
        {
            if (_timerOnlineText != null)
            {
                _timerOnlineText.text = "";
            }
        }
    }

}
