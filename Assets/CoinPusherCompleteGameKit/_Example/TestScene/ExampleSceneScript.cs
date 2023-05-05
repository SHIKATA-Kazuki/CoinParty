using UnityEngine;
using System.Collections;

public class ExampleSceneScript : MonoBehaviour {

// nothing to do, we just need the "ExampleSceneScript" in order to call the "TagHelper" that add tags for examples coin.

    public void DestroyMe()
    {
        DestroyImmediate(this.gameObject);
    }
}
