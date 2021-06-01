using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {
    public List<Item> items;
    public List<GameObject> spawnedObjects;
    public GameObject buySellAnimal;
    public GameObject prefabObject;
    public GameObject prefabTree;
    public GameObject prefabButterfly;
    public List<GameObject> prefabs;
    public DictionaryOfStringAndGameObject prefabDict;
    void Start() {
        UpdateDict();
    }
    public void UpdateDict() {
        foreach (GameObject gameObject in prefabs) {
            prefabDict[gameObject.name.ToUpper()] = gameObject;
        }
    }
    // void Update(){
    //   Spawn();
    // }
    // Start is called before the first frame update
    public void Spawn() {
        foreach (Item a in items) {
            for (int i = 0; i < a.spawnLocations.Length; i++) {
                int random = Random.Range(0, 100);
                if (random <= a.spawnProbability[i]) {
                    generateObject(a.spawnLocations[0], a);
                }
            }
        }
    }
    public void generateObject(Square square, Item item) {
        if (item is Bug) {
            spawnButterfly(square, item as Bug);
            return;
        }
        Vector2 position = new Vector2(Random.Range(square.start.value.x, square.end.value.x), Random.Range(square.start.value.y, square.end.value.y));
        generateObject(position, item);
    }
    public void generateObject(Vector2 position, Item item) {

        GameObject instance = GameObject.Instantiate(prefabObject) as GameObject;
        instance.GetComponent<Object>().item = item;
        instance.GetComponent<Object>().buySellAnimal = buySellAnimal;
        instance.GetComponent<SpriteRenderer>().sprite = item.ItemSprite;
        instance.SetActive(true);
        instance.transform.position = position;
        spawnedObjects.Add(instance);
    }
    public void generateTree(Vector2 location) {
        generatePrefab(location, "tree");
    }
    public void generatePrefab(Vector2 location, string name) {
        if (!prefabDict.ContainsKey(name.ToUpper())) {
            Debug.Log("No Prefab: " + name.ToUpper());
            UpdateDict();
        }
        if (prefabDict.ContainsKey(name.ToUpper())) {
            GameObject instance = GameObject.Instantiate(prefabDict[name.ToUpper()]) as GameObject;
            instance.SetActive(true);
            instance.transform.position = location;
            spawnedObjects.Add(instance);
        }
    }
    public void spawnButterfly(Square square, Bug bug) {
        GameObject instance = Instantiate(prefabButterfly);
        instance.GetComponent<Butterfly>().item = bug;
        instance.GetComponent<Butterfly>().buySellAnimal = buySellAnimal;
        instance.transform.position = new Vector2(Random.Range(square.start.value.x, square.end.value.x), Random.Range(square.start.value.y, square.end.value.y));
        spawnedObjects.Add(instance);

    }
    public void spawnButterfly(Square square, Item item) {
        GameObject instance = Instantiate(prefabButterfly);
        instance.GetComponent<Object>().item = item;
        instance.GetComponent<Object>().buySellAnimal = buySellAnimal;
        instance.transform.position = new Vector2(Random.Range(square.start.value.x, square.end.value.x), Random.Range(square.start.value.y, square.end.value.y));
        spawnedObjects.Add(instance);

    }
}
