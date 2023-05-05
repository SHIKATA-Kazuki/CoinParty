using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class WallsScript : MonoBehaviour {

    private GameManagerScript gameManagerScript;
    private GameObject wallGo;
    private float wallHightMove;
    private float duration=0;
    private float wallSpeed=1;
    private Vector3 maxWallMove;
    private Vector3 startWallPos;
    private Quaternion startWallRot;
    private AudioSource _audio;
    private AudioClip wallSound;
    private Collider[] wallGOCollider;

    public void SetWallSpeed(float newSpeed)
    {
        wallSpeed = newSpeed;
    }

    public void MoveWalls (float durat, float speed)
    {
        wallSpeed = speed;
        duration = +durat;
        StopAllCoroutines();
        StartCoroutine(MoveWallCoroutine());
    }

    void Awake()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerScript>();
        wallHightMove = gameManagerScript.wallMoveHeight;
        wallGo = this.gameObject;
        wallGOCollider = wallGo.GetComponentsInChildren<Collider>();
        foreach(Collider col in wallGOCollider)
        {
            col.isTrigger = true;
        }
        _audio = wallGo.GetComponent<AudioSource>();
        _audio.playOnAwake = false;
        maxWallMove = new Vector3(wallGo.transform.position.x, wallGo.transform.position.y + wallHightMove, wallGo.transform.position.z);
        startWallPos = wallGo.transform.position;
        startWallRot = wallGo.transform.rotation;
    }

    IEnumerator MoveWallCoroutine()
    {
        bool mustMove = true;
        while (mustMove)
        {
            foreach (Collider col in wallGOCollider)
            {
                col.isTrigger = false;
            }
            wallSound = gameManagerScript.wallUpSound;
            _audio.clip = wallSound;
            _audio.Play();
            yield return StartCoroutine(MoveObject(wallGo.transform, wallGo.transform.position, maxWallMove, 3.0f / wallSpeed));
            yield return new WaitForSeconds(duration);
            wallSound = gameManagerScript.wallDownSound;
            _audio.clip = wallSound;
            _audio.Play();
            yield return StartCoroutine(MoveObject(transform, wallGo.transform.position, startWallPos, 3.0f / wallSpeed));
            foreach (Collider col in wallGOCollider)
            {
                col.isTrigger = true;
            }
            mustMove = false;          
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
            thisTransform.rotation = startWallRot;
            yield return null;
        }
    }
    
}
