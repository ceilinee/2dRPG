using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour {
    // Populated on Start to contain all the character game objects in characterHolder
    public List<GameObject> characters;
    public GameObject characterHolder;
    [System.Serializable] public class DictionaryOfIntAndGameObject : SerializableDictionary<int, GameObject> { }

    // Populated on Start to map from character ids to game objects in Mainscene/Characters
    public DictionaryOfIntAndGameObject prefabGameObjectDictionary = new DictionaryOfIntAndGameObject();
    public DictionaryOfIntAndGameObject characterGameObjectDictionary = new DictionaryOfIntAndGameObject();
    public GameObject olderChild;
    public GameObject youngerChild;
    public GameObject baby;

    // Stores saved character trait information
    public Characters curCharacters;
    public GameObject animalList;
    public CurTime curTime;
    public Player player;
    public GameObject spawnAnimal;
    [System.Serializable] public class DictionaryOfTimeAndLocationList : SerializableDictionary<string, List<CharacterPath>> { }
    public DictionaryOfTimeAndLocationList movementDictionaryObject = new DictionaryOfTimeAndLocationList();

    [System.Serializable] public class DictionaryOfTimeAndLocationArray : SerializableDictionary<string, CharacterPath[]> { }

    // Populated on Start and maps from character id to character paths
    public DictionaryOfTimeAndLocationArray movementDictionary = new DictionaryOfTimeAndLocationArray();

    void Start() {
        UpdateCharacters();
        UpdateManager();
        UpdateChildren();
    }
    public void UpdateCharacters() {
        foreach (Transform child in characterHolder.transform) {
            characters.Add(child.gameObject);
        }
    }
    public void UpdateCharacterTraitBasic(Character characterTrait, Character savedCharacter) {
        characterTrait.friendshipScore = savedCharacter.friendshipScore;
        characterTrait.married = savedCharacter.married;
        characterTrait.date = savedCharacter.date;
    }

    // Find the character path (if there is one) the character should take given the current time
    // The path should be in the current scene and happen at time t, where t is the largest s.t. t < curTime
    public CharacterPath FindTime(Character characterTrait) {
        List<string> travelTimes = new List<string>();
        List<CharacterPath> path = new List<CharacterPath>();
        string latestSpawn = null;
        // Find previous spawn
        for (int i = 0; i < characterTrait.path.Length; i++) {
            if (curTime.isCurrentTimeBigger(characterTrait.travelTimes[i]) && characterTrait.path[i].spawn) {
                latestSpawn = characterTrait.path[i].scene;
            }
            if (characterTrait.path[i].scene == SceneManager.GetActiveScene().name) {
                travelTimes.Add(characterTrait.travelTimes[i]);
                path.Add(characterTrait.path[i]);
            }
        }
        if (latestSpawn == SceneManager.GetActiveScene().name) {
            for (int i = 0; i < travelTimes.Count; i++) {
                if (i + 1 == travelTimes.Count) {
                    return path[i];
                } else if (curTime.isCurrentTimeBigger(travelTimes[i]) && curTime.isCurrentTimeSmaller(travelTimes[i + 1])) {
                    return path[i];
                }
            }
        }
        // The character should not be present in the current scene
        return new CharacterPath();
    }

    // Update fields of characterTrait related to movement
    public void UpdateCharacterTraitPath(Character characterTrait, Character savedCharacter, bool savedChar) {
        // Find curChar's current selected path
        characterTrait.selectedPath = FindTime(characterTrait);
        if (savedChar) {
            characterTrait.location = savedCharacter.location;
        }
        // update characterTrait
        characterTrait.characterMovement = new Character.DictionaryOfTimeAndLocation();
        for (int j = 0; j < characterTrait.travelTimes.Length; j++) {
            characterTrait.characterMovement[characterTrait.travelTimes[j]] = characterTrait.path[j];
        }
    }
    public void addToMovementDictionaryObject(Character characterTrait) {
        foreach (KeyValuePair<string, CharacterPath> kvp in characterTrait.characterMovement) {
            if (kvp.Value.scene == SceneManager.GetActiveScene().name) {
                if (!movementDictionaryObject.ContainsKey(kvp.Key)) {
                    movementDictionaryObject[kvp.Key] = new List<CharacterPath>();
                }
                kvp.Value.charId = characterTrait.id;
                movementDictionaryObject[kvp.Key].Add(kvp.Value);
            }
        }
    }
    public void UpdateManager() {
        foreach (GameObject character in characters) {
            var script = character.GetComponent<GenericCharacter>();
            Character characterTrait = script.characterTrait;
            // Transfer saved information from scriptable object curCharacters to character
            if (curCharacters.characterDict.ContainsKey(characterTrait.id)) {
                UpdateCharacterTraitBasic(characterTrait, curCharacters.characterDict[characterTrait.id]);
                UpdateCharacterTraitPath(characterTrait, curCharacters.characterDict[characterTrait.id], true);
            } else { // When game first starts
                curCharacters.characterDict[characterTrait.id] = characterTrait;
                UpdateCharacterTraitPath(characterTrait, new Character(), false);
                // Invariant: characterTrait.selectedPath is the empty path
            }
            // update gameobject
            curCharacters.characterDict[characterTrait.id] = characterTrait;

            prefabGameObjectDictionary[characterTrait.id] = character;
            addToMovementDictionaryObject(characterTrait);
            if (characterTrait.selectedPath.scene == SceneManager.GetActiveScene().name) {
                GameObject instance = GameObject.Instantiate(character) as GameObject;
                // GameObject instance = characters[i];
                instance.transform.position = characterTrait.location;
                instance.GetComponent<GenericCharacter>().spawnAnimal = spawnAnimal;
                instance.GetComponent<GenericCharacter>().characterTrait = characterTrait;
                instance.SetActive(true);
                instance.GetComponent<GenericCharacter>().SetWakeUpTrue();
                characterGameObjectDictionary[characterTrait.id] = instance;
            }
        }
        foreach (KeyValuePair<string, List<CharacterPath>> kvp in movementDictionaryObject) {
            movementDictionary[kvp.Key] = kvp.Value.ToArray();
        }
    }
    public void ClearDailies() {
        curCharacters.ClearDailies();
        foreach (KeyValuePair<int, GameObject> kvp in characterGameObjectDictionary) {
            kvp.Value.GetComponent<GenericCharacter>().ClearDailies();
        }
    }
    public GameObject instantiateChild(Character characterTrait) {
        if (characterTrait.age < 3) {
            //show baby
            GameObject instance = GameObject.Instantiate(baby) as GameObject;
            return instance;
        } else if (characterTrait.age < 10) {
            //show young kid
            GameObject instance = GameObject.Instantiate(youngerChild) as GameObject;
            return instance;
        } else {
            //show old kid
            GameObject instance = GameObject.Instantiate(olderChild) as GameObject;
            return instance;
        }
    }
    public void UpdateChildren() {
        //foreach child of player
        foreach (int id in player.childrenCharId) {
            Character characterTrait = curCharacters.characterDict[id];
            //if child is currently in scene, instantiate 
            if (characterTrait.selectedPath.scene == SceneManager.GetActiveScene().name) {
                GameObject instance = instantiateChild(characterTrait);
                instance.GetComponent<GenericCharacter>().characterTrait = characterTrait;
                instance.transform.position = characterTrait.location;
                addToMovementDictionaryObject(characterTrait);
                instance.GetComponent<GenericCharacter>().spawnAnimal = spawnAnimal;
                instance.GetComponent<GenericCharacter>().characterTrait = characterTrait;
                instance.SetActive(true);
                characterGameObjectDictionary[characterTrait.id] = instance;
            }
        }
    }
    public void ageChildren() {
        //age all existing children
        foreach (int id in player.childrenCharId) {
            //make sure to get existing instances of character
            if (characterGameObjectDictionary.ContainsKey(id)) {
                if (characterGameObjectDictionary[id].GetComponent<GenericCharacter>().characterTrait.age < 16) {
                    characterGameObjectDictionary[id].GetComponent<GenericCharacter>().characterTrait.age += 1;
                }
            }
            //update curCharacters
            else if (curCharacters.characterDict[id].age < 16) {
                curCharacters.characterDict[id].age += 1;
            }
        }
    }

    public void updateCharAnimal() {
        animalList.GetComponent<AnimalList>().deleteCharAnimals();
        foreach (KeyValuePair<int, Character> kvp in curCharacters.characterDict) {
            kvp.Value.currentAnimalId = 0;
            if (characterGameObjectDictionary.ContainsKey(kvp.Key)) {
                characterGameObjectDictionary[kvp.Key].GetComponent<GenericCharacter>().characterTrait.currentAnimalId = 0;
                characterGameObjectDictionary[kvp.Key].GetComponent<GenericCharacter>().getAnimal();
            }
        }
    }
    public void updateCurCharacter() {
        foreach (KeyValuePair<int, GameObject> kvp in characterGameObjectDictionary) {
            var characterTrait = kvp.Value.GetComponent<GenericCharacter>().characterTrait;
            characterTrait.location = kvp.Value.transform.position;
            if (curCharacters.characterDict.ContainsKey(characterTrait.id)) {
                curCharacters.characterDict[characterTrait.id] = characterTrait;
            }
        }
    }
    public void initiateCharacter(int id) {
        GameObject instance = GameObject.Instantiate(prefabGameObjectDictionary[id]) as GameObject;
        instance.GetComponent<GenericCharacter>().spawnAnimal = spawnAnimal;
        instance.SetActive(true);
        characterGameObjectDictionary[id] = instance;
    }

    /// <summary>
    /// `checkCharacter` checks if a character needs to move somewhere at time `time`; if so,
    /// it initializes the location, selectedPath, and transform.position of the character.
    /// It might also initialize a new character and set its currentPoint to 0.
    /// </summary>
    public void checkCharacter(string time) {
        if (movementDictionary.ContainsKey(time)) {
            for (int i = 0; i < movementDictionary[time].Length; i++) {
                // if not initiated, initiate
                if (!characterGameObjectDictionary.ContainsKey(movementDictionary[time][i].charId)) {
                    initiateCharacter(movementDictionary[time][i].charId);
                }
                if (!characterGameObjectDictionary[movementDictionary[time][i].charId].activeInHierarchy) {
                    characterGameObjectDictionary[movementDictionary[time][i].charId].SetActive(true);
                    characterGameObjectDictionary[movementDictionary[time][i].charId].GetComponent<GenericCharacter>().getAnimal();
                }
                var script = characterGameObjectDictionary[movementDictionary[time][i].charId].GetComponent<GenericCharacter>();
                // if charactermovement contains time
                if (script.characterTrait.characterMovement.ContainsKey(time)) {
                    script.characterTrait.selectedPath = movementDictionary[time][i];
                    // wake up
                    script.SetWakeUpTrue();
                }
                if (script.characterTrait.selectedPath.spawn) {
                    script.characterTrait.location = script.characterTrait.selectedPath.src.value;
                    characterGameObjectDictionary[movementDictionary[time][i].charId].transform.position = script.characterTrait.location;
                }
            }
        }
    }
}
