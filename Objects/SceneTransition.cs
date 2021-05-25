using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {
    public SceneInfo sceneinfo;
    // public GameObject characterManager;
    public GameObject animalList;
    public Animals curAnimals;
    public Characters curCharacters;
    public GameObject gameSaveManager;
    public Player player;
    public Vector2 playerPosition;
    public VectorValue playerStorage;
    void Start() {
        gameSaveManager = GameObject.FindGameObjectsWithTag("save")[0];
    }
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            playerStorage.initialValue = sceneinfo.entrance;
            gameSaveManager.GetComponent<GameSaveManager>().updateAnimalAndCharacter();
            // characterManager.GetComponent<CharacterManager>().updateCurCharacter();
            // animalList.GetComponent<AnimalList>().updateList();
            if (!player.dailyScenesVisited.Contains(sceneinfo.id)) {
                player.dailyScenesVisited.Add(sceneinfo.id);
            }
            SceneManager.LoadScene(sceneinfo.sceneName);
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
}
