﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : CustomMonoBehaviour {
    public SceneInfo sceneinfo;
    // public GameObject characterManager;
    public GameObject animalList;
    public Animals curAnimals;
    public Characters curCharacters;
    public GameObject gameSaveManager;
    public Player player;
    // public Vector2 playerPosition;
    public Forest forest;
    public bool inForest;
    public VectorValue playerStorage;

    [SerializeField]
    private SceneInfo mainSceneInfo;

    void Start() {
        gameSaveManager = GameObject.FindGameObjectsWithTag("save")[0];
        animalList = centralController.Get("AnimalList");
    }
    public void PlayerTransition() {
        playerStorage.initialValue = sceneinfo.entrance;
        gameSaveManager.GetComponent<GameSaveManager>().updateAnimalAndCharacter();
        // characterManager.GetComponent<CharacterManager>().updateCurCharacter();
        // animalList.GetComponent<AnimalList>().updateList();
        if (!player.dailyScenesVisited.Contains(sceneinfo.id)) {
            player.dailyScenesVisited.Add(sceneinfo.id);
        }
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Forest" && inForest) {
            if (sceneinfo.sceneName == "Forest") {
                forest.LevelUp();
                playerStorage.initialValue = new Vector2(3 - (forest.width / 2), 6 - (forest.width / 2));
            }
        }
        // TODO: refactor this logic; be able query for mainscene dynamically
        if (scene.name == "MainScene") {
            mainSceneInfo.entrance = centralController.Get("Player").transform.position + new Vector3(0, -1, 0);
            //entering forest
            if (sceneinfo.sceneName == "Forest") {
                StartCoroutine(waitUpdateForest());
                return;
            }
        }
        Loader.Load(sceneName: sceneinfo.sceneName);
    }
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(TagOfPlayer) && !other.isTrigger) {
            PlayerTransition();
        } else if (other.CompareTag("pet") && !other.isTrigger) {
            curAnimals.animalDict[other.gameObject.GetComponent<GenericAnimal>().animalTrait.id].scene = sceneinfo.sceneName;
            curAnimals.animalDict[other.gameObject.GetComponent<GenericAnimal>().animalTrait.id].location = sceneinfo.entrance;
            animalList.GetComponent<AnimalList>().removeAnimal(other.gameObject.GetComponent<GenericAnimal>().animalTrait.id);
            other.gameObject.SetActive(false);
        } else if (other.CompareTag("character") && !other.isTrigger) {
            curCharacters.characterDict[other.gameObject.GetComponent<GenericCharacter>().characterTrait.id].scene = sceneinfo.sceneName;
            curCharacters.characterDict[other.gameObject.GetComponent<GenericCharacter>().characterTrait.id].location = sceneinfo.entrance;
            curCharacters.characterDict[other.gameObject.GetComponent<GenericCharacter>().characterTrait.id].selectedPath = new CharacterPath();
            List<GameObject> list = animalList.GetComponent<AnimalList>().list;
            for (int i = 0; i < list.Count; i++) {
                // Debug.Log(list[i].GetComponent<GenericAnimal>().animalTrait.charId);
                // Debug.Log(other.gameObject.GetComponent<GenericCharacter>().characterTrait.id);
                if (list[i].GetComponent<GenericAnimal>().animalTrait.charId == other.gameObject.GetComponent<GenericCharacter>().characterTrait.id) {
                    Debug.Log("found char animal");
                    animalList.GetComponent<AnimalList>().removeAnimal(list[i].GetComponent<GenericAnimal>().animalTrait.id);
                    list[i].SetActive(false);
                }
            }
            other.gameObject.SetActive(false);
        }
    }
    IEnumerator waitUpdateForest() {
        yield return new WaitForEndOfFrame(); // need this!

        forest.Clear();
        playerStorage.initialValue = new Vector2(3 - (forest.width / 2), 6 - (forest.width / 2));
        yield return new WaitForEndOfFrame();
        Loader.Load(sceneName: sceneinfo.sceneName);
    }
    public void SetSceneInfo(SceneInfo sceneInfo) {
        // sceneInfo.entrance = GetComponent<BoxCollider2D>().transform.position;
        sceneinfo = sceneInfo;
    }
}
