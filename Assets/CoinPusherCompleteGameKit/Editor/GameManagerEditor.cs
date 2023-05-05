using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

public enum ManageXPEnum { Yes, No } 
[CustomEditor(typeof(GameManagerScript))]
public class GameManagerEditor : Editor
{
    private bool clickMachineParts;
    private bool clickPhysic;
    private bool clickgameRules;
    private bool clickplayerdata;
    private bool clickUI;
    private bool clickSound;
    private SerializedObject m_Object;
    private SerializedProperty m_Property;
    private GameObject pusher;
    private GameObject pusherClone;
    private ManageXPEnum _managexpenum;
    private GameObject effectZoneGo;


    private GameManagerScript gameManagerScript;
    void OnEnable()
    {
        gameManagerScript = (GameManagerScript)target;
        m_Object = new SerializedObject(target);
    }

    public override void OnInspectorGUI()
    {
        #region MachinePiece
        if (gameManagerScript.ground == null || gameManagerScript.walls == null || gameManagerScript.pusher == null || gameManagerScript.tapHereZone == null || gameManagerScript.coinCollector == null || gameManagerScript.destroyer == null)
        {
            GUI.backgroundColor = Color.red;
        }
        GUILayout.BeginVertical("box");
        GUILayout.Label("Machine Parts", EditorStyles.boldLabel);
        if (GUILayout.Button((clickMachineParts) ? "Hide: Machine Parts" : "Show: Machine Parts"))
        {
            clickMachineParts = !clickMachineParts;
            EditorPrefs.SetBool("clickMachineParts", clickMachineParts);
        }
        if (clickMachineParts)
        {
            gameManagerScript.ground = EditorGUILayout.ObjectField("Ground GameObject", gameManagerScript.ground, typeof(GameObject), true) as GameObject;
            gameManagerScript.walls = EditorGUILayout.ObjectField("Walls GameObject", gameManagerScript.walls, typeof(GameObject), true) as GameObject;
            pusher = gameManagerScript.pusher = EditorGUILayout.ObjectField("Pusher GameObject", gameManagerScript.pusher, typeof(GameObject), true) as GameObject;
            gameManagerScript.tapHereZone = EditorGUILayout.ObjectField("TapHereZone GameObject", gameManagerScript.tapHereZone, typeof(GameObject), true) as GameObject;
            gameManagerScript.coinCollector = EditorGUILayout.ObjectField("Coin Collector GameObject", gameManagerScript.coinCollector, typeof(GameObject), true) as GameObject;
            gameManagerScript.destroyer = EditorGUILayout.ObjectField("Destroyer GameObject", gameManagerScript.destroyer, typeof(GameObject), true) as GameObject;
            if (gameManagerScript.tapHereZone != null)
            {
                if (gameManagerScript.tapHereZone.GetComponent<TapHereScript>() == null)
                {
                    gameManagerScript.tapHereZone.AddComponent<TapHereScript>();
                }
            }
            if (gameManagerScript.walls != null)
            {
                if (gameManagerScript.walls.GetComponent<WallsScript>() == null)
                {
                    gameManagerScript.walls.AddComponent<WallsScript>();
                }
            }
            if (gameManagerScript.coinCollector != null)
            {
                if (gameManagerScript.coinCollector.GetComponent<CoinCollectorScript>() == null)
                {
                    gameManagerScript.coinCollector.AddComponent<CoinCollectorScript>();
                }
            }
            if (gameManagerScript.destroyer != null)
            {
                if (gameManagerScript.destroyer.GetComponent<CoinDestroyerScript>() == null)
                {
                    gameManagerScript.destroyer.AddComponent<CoinDestroyerScript>();
                }
            }
            if (gameManagerScript.pusher != null)
            {
                if (gameManagerScript.pusher.GetComponent<PusherScript>() == null)
                {
                    gameManagerScript.pusher.AddComponent<PusherScript>();
                    GameObject pusherMaxDistPrefab = AssetDatabase.LoadAssetAtPath("Assets/CoinPusherCompleteGameKit/Prefab/PusherMaxDist.prefab", typeof(GameObject)) as GameObject;
                    pusherClone = Instantiate(pusherMaxDistPrefab, pusher.transform.position, Quaternion.identity) as GameObject;
                    pusherClone.name = "PusherMaxDist";
                    pusherClone.transform.parent = gameManagerScript.pusher.gameObject.transform; 
                } else
                {
                    pusherClone = GameObject.Find("PusherMaxDist");
                    if(pusherClone.transform.position == pusher.transform.position)
                    {
                        GUI.backgroundColor = Color.red;
                        EditorGUILayout.HelpBox("Move the 'Pusher' gameobject to his initial position.\nMove the 'PusherMaxDist' gameobject to this final pusher position", MessageType.Warning);
                    }
                }
            }
        }
        GUILayout.EndVertical();
        GUI.backgroundColor = Color.white;
        #endregion

        #region Physic
        GUILayout.BeginVertical("box");
        GUILayout.Label("Physic", EditorStyles.boldLabel);
        if (GUILayout.Button((clickPhysic) ? "Hide: Physic Setup" : "Show: Physic Setup"))
        {
            clickPhysic = !clickPhysic;
            EditorPrefs.SetBool("clickPhysic", clickPhysic);
        }
        if (clickPhysic)
        {
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("Ground Friction");            
            gameManagerScript.groundFriction = EditorGUILayout.Slider(gameManagerScript.groundFriction, 0.0f, 1.0f);            
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("Ground Bounciness");
            gameManagerScript.groundBounciness = EditorGUILayout.Slider(gameManagerScript.groundBounciness, 0.0f, 1.0f);
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Coin Instantiation Height");
            gameManagerScript.spawnHeightFromGround = EditorGUILayout.FloatField(gameManagerScript.spawnHeightFromGround);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Pusher Speed");
            gameManagerScript.pusherSpeed = EditorGUILayout.FloatField(gameManagerScript.pusherSpeed);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Walls Speed");
            gameManagerScript.wallSpeed = EditorGUILayout.FloatField(gameManagerScript.wallSpeed);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Walls Height");
            gameManagerScript.wallMoveHeight = EditorGUILayout.FloatField(gameManagerScript.wallMoveHeight);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        GUI.backgroundColor = Color.white;
        #endregion

        #region SpecialCoins
        GUILayout.BeginVertical("box");
        GUILayout.Label("Specials Coins", EditorStyles.boldLabel);
        if (GUILayout.Button((clickgameRules) ? "Hide: Specials Coins" : "Show: Specials Coins"))
        {
            clickgameRules = !clickgameRules;
            EditorPrefs.SetBool("gameRules", clickgameRules);
        }
        if (clickgameRules)
        {
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Wall Up Duration");
            gameManagerScript.wallDuration = EditorGUILayout.FloatField(gameManagerScript.wallDuration);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Joker Coin Number");
            gameManagerScript.jokerCoinNumber = EditorGUILayout.IntField(gameManagerScript.jokerCoinNumber);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            gameManagerScript.bigCoinGo = EditorGUILayout.ObjectField("Big Coin GameObject", gameManagerScript.bigCoinGo, typeof(GameObject), true) as GameObject;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Big Coin Explosion Power");
            gameManagerScript.bigCoinPower = EditorGUILayout.FloatField(gameManagerScript.bigCoinPower);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Big Coin Explosion Radius");
            gameManagerScript.bigCoinRadius = EditorGUILayout.FloatField(gameManagerScript.bigCoinRadius);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Multipl XP Duration");
            gameManagerScript.multiplXpDuration = EditorGUILayout.FloatField(gameManagerScript.multiplXpDuration);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Multipl Value Duration");
            gameManagerScript.multiplValueDuration = EditorGUILayout.FloatField(gameManagerScript.multiplValueDuration);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Coin Expander Multiplier");
            gameManagerScript.expanderMultiplicator = EditorGUILayout.FloatField(gameManagerScript.expanderMultiplicator);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Shower Coins Number");
            gameManagerScript.showerCoinNumber = EditorGUILayout.IntField(gameManagerScript.showerCoinNumber);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Shower - Coin Delay");
            gameManagerScript.showerCoinDelay = EditorGUILayout.FloatField(gameManagerScript.showerCoinDelay);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
        GUI.backgroundColor = Color.white;
        #endregion

        #region Player
        GUILayout.BeginVertical("box");
        GUILayout.Label("Player", EditorStyles.boldLabel);
        if (GUILayout.Button((clickplayerdata) ? "Hide: Player" : "Show: Player"))
        {
            clickplayerdata = !clickplayerdata;
            EditorPrefs.SetBool("playerdata", clickplayerdata);
        }
        if (clickplayerdata)
        {
            GUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField("Use SaveManager");
            gameManagerScript.useSave = EditorGUILayout.Toggle(gameManagerScript.useSave);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField("Starting Coin");
            gameManagerScript.startingCoin = EditorGUILayout.IntField(gameManagerScript.startingCoin);
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Tap Action Regen Time");
            gameManagerScript.tapActionRegenTime = EditorGUILayout.FloatField(gameManagerScript.tapActionRegenTime);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Tap Action Limit");
            gameManagerScript.tapActionLimit = EditorGUILayout.IntField(gameManagerScript.tapActionLimit);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Regen Offline Speed");
            gameManagerScript.regenOfflineSpeed = EditorGUILayout.IntField(gameManagerScript.regenOfflineSpeed);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Regen Offline Max Coin");
            gameManagerScript.regenOfflineMaxCoin = EditorGUILayout.IntField(gameManagerScript.regenOfflineMaxCoin);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Regen Online Speed");
            gameManagerScript.regenOnlineSpeed = EditorGUILayout.IntField(gameManagerScript.regenOnlineSpeed);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Regen Online Max Coin");
            gameManagerScript.regenOnlineMaxCoin = EditorGUILayout.IntField(gameManagerScript.regenOnlineMaxCoin);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Manage XP :");
            _managexpenum = (ManageXPEnum)EditorGUILayout.EnumPopup(_managexpenum);
            GUILayout.EndHorizontal();
            if (_managexpenum == ManageXPEnum.Yes)
            {
                gameManagerScript.manageXP = true;                
                m_Property = m_Object.FindProperty("XpPerLevel");
                EditorGUILayout.PropertyField(m_Property, new GUIContent("XP per Level"), true);
                m_Object.ApplyModifiedProperties();
            } else
            {
                gameManagerScript.manageXP = false;
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
        GUI.backgroundColor = Color.white;
        #endregion

        #region Interface
        if (gameManagerScript.coinText == null)
        {
            GUI.backgroundColor = Color.red;
        }
        GUILayout.BeginVertical("box");
        GUILayout.Label("Interface", EditorStyles.boldLabel);
        if (GUILayout.Button((clickUI) ? "Hide: User Interface" : "Show: User Interface"))
        {
            clickUI = !clickUI;
            EditorPrefs.SetBool("clickUI", clickUI);
        }
        if (clickUI)
        {
            GUILayout.BeginVertical("Box");
            gameManagerScript.coinText = EditorGUILayout.ObjectField("Player Coin Text", gameManagerScript.coinText, typeof(Text), true) as Text;
            GUILayout.EndVertical();
            GUI.backgroundColor = Color.white;
            gameManagerScript.timerOnlineText = EditorGUILayout.ObjectField("Timer Regen Text", gameManagerScript.timerOnlineText, typeof(Text), true) as Text;
            gameManagerScript.xpTotalToNextLevelText = EditorGUILayout.ObjectField("XP To Next Level Text", gameManagerScript.xpTotalToNextLevelText, typeof(Text), true) as Text;
            gameManagerScript.xpCurrentToNextLevelText = EditorGUILayout.ObjectField("Player XP Text", gameManagerScript.xpCurrentToNextLevelText, typeof(Text), true) as Text;
            gameManagerScript.xpSlider = EditorGUILayout.ObjectField("Player XP Slider", gameManagerScript.xpSlider, typeof(Slider), true) as Slider;
            gameManagerScript.playerLevelText = EditorGUILayout.ObjectField("Player Level Text", gameManagerScript.playerLevelText, typeof(Text), true) as Text;

            if (GameObject.Find("EffectZone") == null)
            {
                if (GUILayout.Button("Create Effect Zone"))
                {
                    effectZoneGo = AssetDatabase.LoadAssetAtPath("Assets/CoinPusherCompleteGameKit/Prefab/EffectZone.prefab", typeof(GameObject)) as GameObject;
                    GameObject effectZoneGoClone = Instantiate(effectZoneGo, Vector3.zero, Quaternion.identity) as GameObject;
                    effectZoneGoClone.name = "EffectZone";
                    effectZoneGoClone.transform.parent = GameObject.FindGameObjectWithTag("GameController").transform;
                    gameManagerScript.effectZone = effectZoneGoClone;
                }
            } else
            {

            }
            
        }
        GUILayout.EndVertical();
        GUI.backgroundColor = Color.white;
        #endregion

        #region SoundAndEffect
        GUILayout.BeginVertical("box");
        GUILayout.Label("Sounds Effects", EditorStyles.boldLabel);
        if (GUILayout.Button((clickSound) ? "Hide: Sounds Effects" : "Show: Sounds Effects"))
        {
            clickSound = !clickSound;
            EditorPrefs.SetBool("clickSound", clickSound);
        }
        if (clickSound)
        {
            gameManagerScript.wallUpSound = EditorGUILayout.ObjectField("Wall Up Sound", gameManagerScript.wallUpSound, typeof(AudioClip), true) as AudioClip;
            gameManagerScript.wallDownSound = EditorGUILayout.ObjectField("Wall Down Sound", gameManagerScript.wallDownSound, typeof(AudioClip), true) as AudioClip;
            gameManagerScript.tapEmptySound = EditorGUILayout.ObjectField("Tap Empty Sound", gameManagerScript.tapEmptySound, typeof(AudioClip), true) as AudioClip;
        }
        GUILayout.EndVertical();
        GUI.backgroundColor = Color.white;
        #endregion


        EditorUtility.SetDirty(target);
    }
}
