using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class DictionaryOfStringAndGameObject : SerializableDictionary<string, GameObject> { }
[System.Serializable]
public class GameObjectAndMaxMin {
    public Maxmin maxMin;
    public List<GameObject> areaGameObjects;
    public void turnOff() {
        foreach (GameObject gameObject in areaGameObjects) {
            gameObject.SetActive(false);
        }
    }
    public void turnOn() {
        foreach (GameObject gameObject in areaGameObjects) {
            gameObject.SetActive(true);
        }
    }
}

//This controller contains references to all canvas/controller/SO/GameObjects 
public class CentralController : MonoBehaviour {
    //Canvas Objects
    public DictionaryOfStringAndGameObject centralDictionary;
    public GameObjectAndMaxMin townObjects;
    public GameObjectAndMaxMin farmObjects;
    public GameObjectAndMaxMin beachObjects;
    public GameObjectAndMaxMin forestObjects;
    public GameObjectAndMaxMin town2Objects;
    public GameObjectAndMaxMin suburbObjects;

    public List<GameObjectAndMaxMin> areaObjects;

    // Start is called before the first frame update
    void Start() {
        areaObjects = new List<GameObjectAndMaxMin> { townObjects, farmObjects, beachObjects, forestObjects, town2Objects, suburbObjects };
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in allObjects) {
            centralDictionary[gameObject.name] = gameObject;
            if (gameObject.tag == "decoration") {
                decorationList(gameObject);
            }
        }
    }

    public void decorationList(GameObject gameObject) {
        foreach (GameObjectAndMaxMin area in areaObjects) {
            if (area.maxMin.withinMaxMin(gameObject.transform.position)) {
                area.areaGameObjects.Add(gameObject);
                return;
            }
        }
    }

    // Update is called once per frame
    // Update, check location of player and if we should turn nearby gameObjects on 
    void Update() {
        if (!centralDictionary["Player"]) {
            centralDictionary["Player"] = GameObject.FindWithTag("Player");
        }
        Vector2 position = centralDictionary["Player"].transform.position;
        TurnOffGameObjects(position);
        TurnOnGameObjects(position);
    }
    public void TurnOffGameObjects(Vector2 position) {
        foreach (GameObjectAndMaxMin area in areaObjects) {
            if (!area.maxMin.withinMaxMin(position, 10)) {
                area.turnOff();
            }
        }
    }
    public void TurnOnGameObjects(Vector2 position) {
        foreach (GameObjectAndMaxMin area in areaObjects) {

            if (area.maxMin.withinMaxMin(position, 10)) {
                area.turnOn();
            }
        }
    }
}
