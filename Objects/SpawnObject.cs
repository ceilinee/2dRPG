using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {
    public List<Item> items;
    public List<GameObject> spawnedObjects;
    public GameObject buySellAnimal;
    public GameObject prefabObject;
    public GameObject prefabButterfly;

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
        if (item.name == "Butterfly") {
            spawnButterfly(square, item);
            return;
        }
        GameObject instance = GameObject.Instantiate(prefabObject) as GameObject;
        instance.GetComponent<Object>().item = item;
        instance.GetComponent<Object>().buySellAnimal = buySellAnimal;
        instance.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
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
