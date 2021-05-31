using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable] public class DictionaryOfStringAndGameObject : SerializableDictionary<string, GameObject> { }

// GameObjectAndMaxMin stores game objects that are within an area defined by field maxMin
// Responsible for turning these game objects active or inactive
[System.Serializable]
public class GameObjectAndMaxMin {
    public Maxmin maxMin;
    public List<GameObject> areaGameObjects;

    // An area is turned off iff all the game objects within that area are inactive
    public bool areaTurnedOff;

    public void TurnOff() {
        areaTurnedOff = true;
        foreach (GameObject gameObject in areaGameObjects) {
            gameObject.SetActive(false);
        }
    }
    public void TurnOn() {
        areaTurnedOff = false;
        foreach (GameObject gameObject in areaGameObjects) {
            gameObject.SetActive(true);
        }
    }
}

//This controller contains references to all canvas/controller/SO/GameObjects 
public class CentralController : CustomMonoBehaviour {
    // Populated from listGameObject in Awake
    public DictionaryOfStringAndGameObject centralDictionary;
    public GameObjectAndMaxMin townObjects;
    public GameObjectAndMaxMin farmObjects;
    public GameObjectAndMaxMin beachObjects;
    public GameObjectAndMaxMin forestObjects;
    public GameObjectAndMaxMin town2Objects;
    public GameObjectAndMaxMin suburbObjects;

    [Header("References that can be accessed anywhere in the scene through the CentralController")]
    public List<GameObject> listGameObject;

    public List<GameObjectAndMaxMin> areaObjects;

    // Start is called before the first frame update
    void Awake() {
        SetCentralController(this);

        areaObjects = new List<GameObjectAndMaxMin> { townObjects, farmObjects, beachObjects, forestObjects, town2Objects, suburbObjects };
        foreach (GameObject gameObject in listGameObject) {
            Assert.IsFalse(centralDictionary.ContainsKey(gameObject.name), "Duplicate Key");
            centralDictionary[gameObject.name] = gameObject;
        }
        // FindGameObjectsWithTag only returns Game Objects that are active
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("decoration");
        foreach (GameObject gameObject in allObjects) {
            DecorationList(gameObject);
        }
    }

    // Add gameObject to areaObjects
    // We only want to selectively turn on / off game objects that are decorations
    public void DecorationList(GameObject gameObject) {
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
        if (centralDictionary.ContainsKey("Player")) {
            Vector2 position = centralDictionary["Player"].transform.position;
            TurnOffGameObjects(position);
            TurnOnGameObjects(position);
        }
    }
    public void TurnOffGameObjects(Vector2 position) {
        foreach (GameObjectAndMaxMin area in areaObjects) {
            if (!area.areaTurnedOff && !area.maxMin.withinMaxMin(position, 10)) {
                area.TurnOff();
            }
        }
    }
    public void TurnOnGameObjects(Vector2 position) {
        foreach (GameObjectAndMaxMin area in areaObjects) {
            if (area.areaTurnedOff && area.maxMin.withinMaxMin(position, 10)) {
                area.TurnOn();
            }
        }
    }

    public GameObject Get(string gameObjectName) {
        Assert.IsTrue(centralDictionary.ContainsKey(gameObjectName),
            $"No game object {gameObjectName} found in central dictionary");
        return centralDictionary[gameObjectName];
    }
}
