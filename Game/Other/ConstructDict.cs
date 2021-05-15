using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructDict : MonoBehaviour {
    public SceneInfos sceneInfos;
    // Start is called before the first frame update
    void Start() {
        sceneInfos.updateSceneDict();
    }

    // Update is called once per frame
    void Update() {

    }
}
