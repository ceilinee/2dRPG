using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour {
    public List<GameObject> characters;
    public GameObject characterHolder;
    [System.Serializable] public class DictionaryOfIntAndGameObject : SerializableDictionary<int, GameObject> { }
    public DictionaryOfIntAndGameObject prefabGameObjectDictionary = new DictionaryOfIntAndGameObject();
    public DictionaryOfIntAndGameObject characterGameObjectDictionary = new DictionaryOfIntAndGameObject();
    public GameObject olderChild;
    public GameObject youngerChild;
    public GameObject baby;
    public Characters curCharacters;
    public GameObject animalList;
    public CurTime curTime;
    public Player player;
    public GameObject spawnAnimal;
    [System.Serializable] public class DictionaryOfTimeAndLocationList : SerializableDictionary<string, List<CharacterPath>> { }
    public DictionaryOfTimeAndLocationList movementDictionaryObject = new DictionaryOfTimeAndLocationList();
    [System.Serializable] public class DictionaryOfTimeAndLocationArray : SerializableDictionary<string, CharacterPath[]> { }
    public DictionaryOfTimeAndLocationArray movementDictionary = new DictionaryOfTimeAndLocationArray();

    void Start() {
        //for all characters
        updateCharacters();
        UpdateManager();
        UpdateChildren();
    }
    public void updateCharacters() {
        foreach (Transform child in characterHolder.transform) {
            characters.Add(child.gameObject);
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
    public void UpdateManager() {
        for (int i = 0; i < characters.Count; i++) {
            var script = characters[i].GetComponent<GenericCharacter>();
            var characterTrait = script.characterTrait;
            //Add to curCharacters
            script.curCharacters = curCharacters;
            if (curCharacters.characterDict.ContainsKey(characterTrait.id)) {
                updateCharacterTrait(characterTrait, curCharacters.characterDict[characterTrait.id]);
                //update curChar with values from curCharacters
                // updateCharacterTraitPath(characterTrait, new Character(), false);
                updateCharacterTraitPath(characterTrait, curCharacters.characterDict[characterTrait.id], true);
            } else {
                curCharacters.characterDict[characterTrait.id] = characterTrait;
                updateCharacterTraitPath(characterTrait, new Character(), false);
            }
            //update gameobject
            curCharacters.characterDict[characterTrait.id] = characterTrait;

            //Add to movementDictionaryObject
            prefabGameObjectDictionary[characterTrait.id] = characters[i];
            addToMovementDictionaryObject(characterTrait);
            if (characterTrait.selectedPath.scene == SceneManager.GetActiveScene().name) {
                GameObject instance = GameObject.Instantiate(characters[i]) as GameObject;
                // GameObject instance = characters[i];
                instance.transform.position = characterTrait.location;
                instance.GetComponent<GenericCharacter>().spawnAnimal = spawnAnimal;
                instance.GetComponent<GenericCharacter>().characterTrait = characterTrait;
                instance.SetActive(true);
                characterGameObjectDictionary[characterTrait.id] = instance;
            }
        }
        foreach (KeyValuePair<string, List<CharacterPath>> kvp in movementDictionaryObject) {
            movementDictionary[kvp.Key] = kvp.Value.ToArray();
        }
    }
    public void updateCharAnimal() {
        animalList.GetComponent<AnimalList>().deleteCharAnimals();
        foreach (KeyValuePair<int, Character> kvp in curCharacters.characterDict) {
            kvp.Value.currentAnimalId = 0;
            characterGameObjectDictionary[kvp.Key].GetComponent<GenericCharacter>().characterTrait.currentAnimalId = 0;
            characterGameObjectDictionary[kvp.Key].GetComponent<GenericCharacter>().getAnimal();
        }
    }
    public CharacterPath FindTime(Character characterTrait) {
        List<string> _travelTimes = new List<string>();
        List<CharacterPath> _path = new List<CharacterPath>();
        string latestSpawn = null;
        //find previous spawn
        for (int i = 0; i < characterTrait.path.Length; i++) {
            if (curTime.isCurrentTimeBigger(characterTrait.travelTimes[i]) && characterTrait.path[i].spawn) {
                latestSpawn = characterTrait.path[i].scene;
            }
            if (characterTrait.path[i].scene == SceneManager.GetActiveScene().name) {
                _travelTimes.Add(characterTrait.travelTimes[i]);
                _path.Add(characterTrait.path[i]);
            }
        }
        if (latestSpawn == SceneManager.GetActiveScene().name) {
            for (int i = 0; i < _travelTimes.Count; i++) {
                if (i + 1 == _travelTimes.Count) {
                    return _path[i];
                } else if (curTime.isCurrentTimeBigger(_travelTimes[i]) && curTime.isCurrentTimeSmaller(_travelTimes[i + 1])) {
                    return _path[i];
                }
            }
        }
        return new CharacterPath();
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
        // GameObject instance = prefabGameObjectDictionary[id];
        instance.GetComponent<GenericCharacter>().spawnAnimal = spawnAnimal;
        instance.SetActive(true);
        characterGameObjectDictionary[id] = instance;
    }
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
                    // wake up
                    script.SetWakeUpTrue();
                    // if char not at last point of selected path
                    if (script.characterTrait.selectedPath.pathArray.Length > 0 &&
                    Vector3.Distance(characterGameObjectDictionary[movementDictionary[time][i].charId].transform.position, script.characterTrait.selectedPath.pathArray[script.characterTrait.selectedPath.pathArray.Length - 1].value) > 1 &&
                    script.characterTrait.selectedPath.scene == movementDictionary[time][i].scene) {
                        CharacterPath charPath = mergeCharPath(script.characterTrait.selectedPath, movementDictionary[time][i]);
                        script.characterTrait.selectedPath = charPath;
                    } else {
                        script.characterTrait.currentPoint = 0;
                        script.characterTrait.selectedPath = movementDictionary[time][i];
                    }
                }
                if (script.characterTrait.currentPoint == script.characterTrait.selectedPath.pathArray.Length - 1 &&
                Vector3.Distance(script.characterTrait.selectedPath.pathArray[script.characterTrait.currentPoint].value, curCharacters.characterDict[script.characterTrait.id].location) <= 1) {
                    script.characterTrait.location = script.characterTrait.selectedPath.pathArray[script.characterTrait.currentPoint].value;
                    characterGameObjectDictionary[movementDictionary[time][i].charId].transform.position = script.characterTrait.location;
                } else if (script.characterTrait.selectedPath.spawn) {
                    script.characterTrait.location = script.characterTrait.selectedPath.pathArray[System.Math.Min(System.Math.Max(script.characterTrait.currentPoint - 1, 0), script.characterTrait.selectedPath.pathArray.Length - 1)].value;
                    characterGameObjectDictionary[movementDictionary[time][i].charId].transform.position = script.characterTrait.location;
                }
            }
        }
    }
    public CharacterPath mergeCharPath(CharacterPath first, CharacterPath second) {
        CharacterPath charPath = new CharacterPath();
        List<VectorPoints> tempPath = new List<VectorPoints>();
        for (int j = 0; j < first.pathArray.Length; j++) {
            tempPath.Add(first.pathArray[j]);
        }
        for (int j = 0; j < second.pathArray.Length; j++) {
            tempPath.Add(second.pathArray[j]);
        }
        charPath.pathArray = tempPath.ToArray();
        charPath.charId = second.charId;
        charPath.action = second.action;
        charPath.scene = second.scene;
        return charPath;
    }
    public void updateCharacterTrait(Character characterTrait, Character savedCharacter) {
        characterTrait.friendshipScore = savedCharacter.friendshipScore;
        characterTrait.married = savedCharacter.married;
        characterTrait.date = savedCharacter.date;
    }
    public void updateCharacterTraitPath(Character characterTrait, Character savedCharacter, bool savedChar) {
        CharacterPath prevPath = new CharacterPath();
        if (savedChar) {
            prevPath = savedCharacter.selectedPath;
        }
        //find curChar's current selected path
        characterTrait.selectedPath = FindTime(characterTrait);
        //reset current point if selected path is different
        if (characterTrait.selectedPath != prevPath) {
            characterTrait.currentPoint = 0;
        } else {
            if (savedChar) {
                characterTrait.currentPoint = savedCharacter.currentPoint;
            }
        }
        if (characterTrait.selectedPath.scene != null && characterTrait.selectedPath.pathArray.Length > 0) {
            //if character is at the last point then put them there, else put them at current point - 1
            if (characterTrait.currentPoint == characterTrait.selectedPath.pathArray.Length - 1 && Vector3.Distance(characterTrait.selectedPath.pathArray[characterTrait.currentPoint].value, curCharacters.characterDict[characterTrait.id].location) <= 1) {
                characterTrait.location = characterTrait.selectedPath.pathArray[characterTrait.currentPoint].value;
            } else {
                characterTrait.location = characterTrait.selectedPath.pathArray[System.Math.Min(System.Math.Max(characterTrait.currentPoint - 1, 0), characterTrait.selectedPath.pathArray.Length - 1)].value;
            }
        }
        //update characterTrait
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
}
