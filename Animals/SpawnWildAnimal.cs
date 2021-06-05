using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnWildAnimal : MonoBehaviour {
    public GameObject spawnAnimal;
    public Animals wildAnimals;
    public List<Square> squares;
    public GameObject breedScript;
    public AnimalBreed animalBreed;
    public bool spawn = false;
    // void Update(){
    //   Spawn();
    // }
    public void Start() {
        spawnAnimal.GetComponent<SpawnAnimal>().defineAnimalDictionary();
        foreach (KeyValuePair<int, Animal> kvp in wildAnimals.animalDict) {
            if (kvp.Value.age >= 0 && kvp.Value.scene == SceneManager.GetActiveScene().name) {
                spawnAnimal.GetComponent<SpawnAnimal>().Spawn(kvp.Value);
            }
        }
        if (spawn) {
            Spawn();
        }
    }
    // Start is called before the first frame update
    public void Spawn() {
        foreach (Square square in squares) {
            for (int i = 0; i < square.types.Length; i++) {
                int random = Random.Range(0, 100);
                if (random <= square.probability[i]) {
                    generateAnimal(square, square.types[i]);
                }
            }
        }
    }
    public void generateAnimal(Square square, string type, int rarity = 0) {
        Vector2 location = new Vector2(Random.Range(square.start.value.x, square.end.value.x), Random.Range(square.start.value.y, square.end.value.y));
        generateAnimal(location, type, rarity);
    }
    public void generateAnimal(Vector2 location, List<string> notTypes = null, int rarity = 0) {
        string type = breedScript.GetComponent<BreedScript>().getType(notTypes);
        generateAnimal(location, type, rarity);
    }
    public void generateAnimal(Vector2 location, string type, int rarity = 0) {
        int random = Random.Range(0, 100);
        int id = 0;
        if (random <= 75 || rarity <= 1) {
            id = breedScript.GetComponent<BreedScript>().RandomShopAnimal(paramAnimals: wildAnimals, type);
        } else {
            id = breedScript.GetComponent<BreedScript>().RandomBreedAnimal(animalBreed.getRandomBreed(type: type, rarity: rarity), paramAnimals: wildAnimals, curType: type);
        }
        Animal animal = wildAnimals.animalDict[id];
        animal.location = location;
        animal.wild = true;
        animal.age = Random.Range(1, 20);
        animal.love = -20 - (animal.cost / 4);
        spawnAnimal.GetComponent<SpawnAnimal>().Spawn(animal);
    }
}
