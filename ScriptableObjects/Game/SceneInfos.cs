using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// SceneInfos represents a group of scene infos
[CreateAssetMenu]
[System.Serializable]
public class SceneInfos : ScriptableObject {
    public List<SceneInfo> sceneInfoArray;
    [System.Serializable] public class DictionaryOfSceneInfo : SerializableDictionary<int, SceneInfo> { }

    // Maps from scene id to scene info
    public DictionaryOfSceneInfo sceneDict = new DictionaryOfSceneInfo();
    public void UpdateSceneDict() {
        foreach (SceneInfo scene in sceneInfoArray) {
            sceneDict[scene.id] = scene;
        }
    }
}
