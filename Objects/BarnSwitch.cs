﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnSwitch : MonoBehaviour {
    public SceneInfo barnInfo;
    public SpriteRenderer barn;
    public SpriteRenderer barnSwitch;
    public Sprite openImg;
    public Sprite closedImg;
    public Sprite openSwitchImg;
    public Sprite closedSwitchImg;
    public bool playerInRange;
    public Transform target;
    public GameObject animalCollision;
    public SceneInfo currentSceneInfo;
    public GameObject spawnAnimal;
    public Animals curAnimals;
    public GameObject animalList;
    public Animals newAnimals;
    public GameObject confirmationModal;

    void Start() {
        if (barnInfo.open) {
            StartCoroutine(waitSpawn());
        } else {
            closeBarn();
        }
    }
    public void closeBarn() {
        barnInfo.open = false;
        barn.sprite = closedImg;
        barnSwitch.sprite = closedSwitchImg;
        animalCollision.SetActive(true);
    }
    public void openBarn() {
        Debug.Log("open barn");
        barnInfo.open = true;
        barn.sprite = openImg;
        barnSwitch.sprite = openSwitchImg;
        animalCollision.SetActive(false);
    }
    public void confirm() {
        Time.timeScale = 1;
        StartCoroutine(waitSpawn());
    }
    public void cancel() {
        Time.timeScale = 1;
    }
    void Update() {
        if (Vector3.Distance(target.position, transform.position) <= 2) {
            playerInRange = true;
        } else if (Vector3.Distance(target.position, transform.position) > 2 && playerInRange == true) {
            playerInRange = false;
        }
        // if(barnInfo.open && barnSwitch != openSwitchImg){
        //   StartCoroutine(waitSpawn());
        // }
        if (Input.GetKeyUp(KeyCode.Space) && playerInRange) {
            //call animals home
            if (!barnInfo.open) {
                Time.timeScale = 0;
                confirmationModal.GetComponent<Confirmation>().initiateConfirmation(
                "Call the animals currently inside " + barnInfo.sceneName + " out?",
                (() => confirm()),
                (() => cancel()));
            } else if (barnInfo.open) {
                closeBarn();
            }
        }
    }
    IEnumerator waitSpawn() {
        Debug.Log("spawn");
        openBarn();
        newAnimals = Instantiate(curAnimals);
        foreach (GameObject animal in animalList.GetComponent<AnimalList>().list) {
            newAnimals.removeExistingAnimal(animal.GetComponent<GenericAnimal>().animalTrait.id);
        }
        yield return new WaitForSeconds(1);
        foreach (KeyValuePair<int, Animal> kvp in newAnimals.animalDict) {
            if (kvp.Value.scene == barnInfo.sceneName && !kvp.Value.characterOwned && barnInfo.open) {
                kvp.Value.location = currentSceneInfo.entrance;
                kvp.Value.target = currentSceneInfo.entrance - new Vector2(0, Random.Range(2, 6));
                kvp.Value.scene = currentSceneInfo.sceneName;
                curAnimals.animalDict[kvp.Value.id] = kvp.Value;
                spawnAnimal.GetComponent<SpawnAnimal>().Spawn(kvp.Value);
                yield return new WaitForSeconds(Random.Range(0, 5));
            }
        }
    }
}
