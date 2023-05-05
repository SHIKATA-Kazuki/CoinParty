using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PusherScript))]
public class PusherScriptEditor : Editor
{

    private GameObject pusherMaxDist;
    PusherScript _pusherScript;

    void OnEnable()
    {
        _pusherScript = (PusherScript)target;
        if (GameObject.Find("PusherMaxDist") == null)
        {
            pusherMaxDist = AssetDatabase.LoadAssetAtPath("Assets/CoinPusherCompleteGameKit/Prefab/PusherMaxDist.prefab", typeof(GameObject)) as GameObject;
            GameObject pusherMaxDistClone = Instantiate(pusherMaxDist, Vector3.zero, Quaternion.identity) as GameObject;
            pusherMaxDistClone.name = "PusherMaxDist";
            GameObject pusherGo = _pusherScript.GetPusherGo();
            pusherMaxDistClone.transform.parent = pusherGo.transform;
            pusherMaxDistClone.transform.localPosition = new Vector3(0, 0, 0);
            pusherMaxDist = pusherMaxDistClone;
            _pusherScript.maxPusherMoveGO = pusherMaxDist;
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Move the 'Pusher' gameobject to his initial position.\nMove the 'PusherMaxDist' gameobject to this final pusher position", MessageType.Info);
    }

}