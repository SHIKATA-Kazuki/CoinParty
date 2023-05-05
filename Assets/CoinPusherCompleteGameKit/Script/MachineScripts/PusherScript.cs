using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PusherScript : MonoBehaviour
{

    private float pusherSpeed = 1;
    private GameObject pusherGo;
    public GameObject maxPusherMoveGO;
    private Vector3 maxPusherMove;
    private Vector3 startPusherPos;
    private Quaternion startPusherRot; // just in case...

    public GameObject GetPusherGo()
    {
        pusherGo = this.gameObject;
        return pusherGo;
    }

   /* void Awake()
    {           
        maxPusherMoveGO = GameObject.Find("PusherMaxDist");
        maxPusherMove = maxPusherMoveGO.transform.position;
        pusherGo = this.gameObject;
        pusherGo.layer = 2;// layer 2 = "IgnoreRaycast", The player can instantiate coin even if the pusher is over the "TapHereZone"
        startPusherPos = pusherGo.transform.position;
        startPusherRot = pusherGo.transform.rotation;
        
    }*/

    public void InitPusherData() //
    {
        maxPusherMoveGO = GameObject.Find("PusherMaxDist");
        maxPusherMove = maxPusherMoveGO.transform.position;
        pusherGo = this.gameObject;
        pusherGo.layer = 2;// layer 2 = "IgnoreRaycast", The player can instantiate coin even if the pusher is over the "TapHereZone"
        startPusherPos = pusherGo.transform.position;
        startPusherRot = pusherGo.transform.rotation;
    }
    public void SetPusherSpeed(float newSpeed)
    {
        pusherSpeed = newSpeed;
        StartCoroutine(MovePusher());
    }

    IEnumerator MovePusher()
    {
        while (true)
        {
            pusherGo = this.gameObject;
            yield return StartCoroutine(MoveObject(pusherGo.transform, startPusherPos, maxPusherMove, 3.0f/pusherSpeed));
            yield return StartCoroutine(MoveObject(transform, maxPusherMove, startPusherPos, 3.0f / pusherSpeed));
        }
    }
    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            thisTransform.rotation = startPusherRot;
            yield return null;
        }
    }

}
