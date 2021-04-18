using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class SceneInfos : ScriptableObject
{
  public List<int> sceneArray;
  public List<SceneInfo> sceneInfoArray;
  [System.Serializable] public class DictionaryOfSceneInfo : SerializableDictionary<int, SceneInfo> {}
  public DictionaryOfSceneInfo sceneDict = new DictionaryOfSceneInfo();

  public void updateSceneDict(){
    foreach(SceneInfo scene in sceneInfoArray){
      sceneDict[scene.id] = scene;
    }
  }
  public void RemoveBuilding(int building){
    sceneArray.Remove(building);
  }
  public void AddBuilding(int building){
    sceneArray.Add(building);
  }
}
