using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Animals : ScriptableObject {
    [System.Serializable] public class DictionaryOfAnimals : SerializableDictionary<int, Animal> { }
    public DictionaryOfAnimals animalDict = new DictionaryOfAnimals();
    public string breedName;

    public void addAnimal(string newAnimalName,
    string newType, int newId, Vector2 newLocation, int newAge, string newMood,
    Animal.StringAndAnimalColor newColoring, bool newFollow, string newGender,
    string newHome, int newCost, bool newPregnant, int newDeliveryDate, int[] newBabyId, float newLove, string newBreed, string scene, bool charOwned, int charId, AnimalColors animalColors, Personality personality, int _momId, int _dadId) {
        Animal newAnimal = new Animal();
        newAnimal.createAnimal(newAnimalName,
        newType, newId, newLocation, newAge, newMood,
        newColoring, newFollow, newGender, newHome, newCost, newPregnant, newDeliveryDate, newBabyId, newLove, newBreed, scene, charOwned, charId, animalColors, personality, _momId, _dadId);
        animalDict[newId] = newAnimal;
    }

    public bool ContainsAnimal(int id) {
        return animalDict.ContainsKey(id);
    }

    public void addExistingAnimal(Animal newAnimal) {
        animalDict[newAnimal.id] = newAnimal;
    }
    public void ClearDailies() {
        foreach (KeyValuePair<int, Animal> kvp in animalDict) {
            kvp.Value.walked = false;
            kvp.Value.presentsDaily = 0;
        }
    }
    public void Clear() {
        animalDict = new DictionaryOfAnimals();
        breedName = null;
    }
    public void removeExistingAnimal(int id) {
        animalDict.Remove(id);
    }
    public void ageAnimals(int time) {
        foreach (KeyValuePair<int, Animal> kvp in animalDict) {
            if (kvp.Value.age != -1) {
                kvp.Value.age += time;
            }
        }
    }
    public void dailyAnimalUpdate() {
        foreach (KeyValuePair<int, Animal> kvp in animalDict) {
            if (kvp.Value.presentsDaily == 0 && !kvp.Value.walked) {
                kvp.Value.mood = "Feeling lonely, would like to go on a walk";
                kvp.Value.moodId = "sad";
                kvp.Value.health -= 2;
            }
            if (kvp.Value.presentsDaily >= 1 || kvp.Value.walked || kvp.Value.health >= 90) {
                kvp.Value.mood = "Feeling content";
                kvp.Value.moodId = "2";
                kvp.Value.health += 1;
            } else if (kvp.Value.presentsDaily >= 1 && kvp.Value.walked) {
                kvp.Value.mood = "Feeling quite pleased!";
                kvp.Value.moodId = "3";
                kvp.Value.health += 2;
            } else if (kvp.Value.presentsDaily == 3 && kvp.Value.walked) {
                kvp.Value.mood = "Feeling ecstatic and lucky!";
                kvp.Value.moodId = "4";
                kvp.Value.health += 3;
            }
            if (kvp.Value.health <= 30) {
                kvp.Value.mood = "Feeling under the weather";
                kvp.Value.moodId = "";
            }
            if (kvp.Value.health <= 20) {
                kvp.Value.mood = "Feeling sick";
                kvp.Value.moodId = "sick";
            }
            if (kvp.Value.health <= 10) {
                kvp.Value.mood = "Needs to go see the vet";
                kvp.Value.moodId = "sick";
            }
        }
    }
    public void updateAnimal(Animal animal) {
        if (animalDict.ContainsKey(animal.id)) {
            animalDict[animal.id] = animal;
        }
    }
}
