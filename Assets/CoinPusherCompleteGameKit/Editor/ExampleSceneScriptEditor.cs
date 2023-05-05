using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ExampleSceneScript))]
public class ExampleSceneScriptEditor : Editor {

    void OnEnable()
    {
        TagHelper.AddTag("CoinWall");
        TagHelper.AddTag("CoinPrize");
        TagHelper.AddTag("CoinJoker");
        TagHelper.AddTag("CoinStandard");
        TagHelper.AddTag("BigCoin");
        TagHelper.AddTag("CoinBigToken");
        TagHelper.AddTag("CoinCollector");
        TagHelper.AddTag("Destroyer");
        TagHelper.AddTag("Ground");
        TagHelper.AddTag("DoubleXP");
        TagHelper.AddTag("DoubleValue");
        TagHelper.AddTag("CoinShower");
        TagHelper.AddTag("CoinExpand");
        GameObject.Find("ClickOnMePlease_IWillDisappear").GetComponent<ExampleSceneScript>().DestroyMe();
    }


}
