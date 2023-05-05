using UnityEngine;
using System.Collections;
using UnityEditor;
using GoogleMobileAds.Api;

[CustomEditor(typeof(AdmobScript))]
public class AdmobScriptEditor : Editor
{

    private AdmobScript admobScript;

    void OnEnable()
    {
        admobScript = (AdmobScript)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical("Box");
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Android Ad ID :");
        admobScript.AndroidAdID = EditorGUILayout.TextField(admobScript.AndroidAdID);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("IOS Ad ID :");
        admobScript.IosAdId = EditorGUILayout.TextField(admobScript.IosAdId);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Ad Type:");
        admobScript._type = (AdType)EditorGUILayout.EnumPopup(admobScript._type);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        switch (admobScript._type)
        {
            case AdType.InterstitialAd:
                //do nothing!!
                break;
            case AdType.BannerAd:
                GUILayout.BeginVertical("Box");
                EditorGUILayout.LabelField("Banner Option:", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Banner size:");
                admobScript._size = (Size)EditorGUILayout.EnumPopup(admobScript._size);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Banner position:");
                admobScript.position = (AdPosition)EditorGUILayout.EnumPopup(admobScript.position);
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                break;
        }
        EditorUtility.SetDirty(target);
    }
}
