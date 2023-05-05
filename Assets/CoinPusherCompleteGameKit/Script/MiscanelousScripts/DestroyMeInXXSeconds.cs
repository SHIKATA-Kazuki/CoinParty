using UnityEngine;
using System.Collections;

public class DestroyMeInXXSeconds : MonoBehaviour {

    public float waitingTime;
    
    void Awake()
    {
        StartCoroutine(StopEmitter());
    }
    IEnumerator StopEmitter()
    {
        yield return new WaitForSeconds(waitingTime);
        Destroy(this.gameObject);
    }
}
