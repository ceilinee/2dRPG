using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
// using UnityEditor;

public class SpawnAnimal : MonoBehaviour {
    //store gameObject reference
    public List<GameObject> animalList;
    public AnimalMood animalMood;
    [System.Serializable] public class DictionaryOfAnimals : SerializableDictionary<string, GameObject> { }
    public DictionaryOfAnimals animalDictionary = new DictionaryOfAnimals();
    public GameObject animalModal;
    public Animals curAnimals;
    public Animals wildAnimals;
    public GameObject water;
    public Inventory playerInventory;
    public AnimalBreed animalBreeds;
    public AnimalColors animalColors;
    public Player player;
    // public GameObject BreedAnimal;
    public GameObject animalGameObject;
    public GameObject cameraGameObject;
    public static SpawnAnimal animalInstance;
    public Personalities personalityList;

    [SerializeField]
    private PlacedBuildings placedBuildings;

    void Start() {
        defineAnimalDictionary();
        // updateAnimalMood();
        SpawnAll();
    }
    public void updateAnimalMood() {
        //only run when updating animal mood 
        for (int i = 0; i < animalMood.reactionIds.Length; i++) {
            animalMood.reactions[animalMood.reactionIds[i]] = animalMood.giftReactions[i];
        }
        AnimalMood.DictionaryOfMoodArray curarray = new AnimalMood.DictionaryOfMoodArray();
        for (int i = 0; i < 10; i++) {
            animalMood.personalityMoodDict[i] = new AnimalMood.DictionaryOfMoodArray();
            foreach (KeyValuePair<string, GameObject> kvp in animalDictionary) {
                animalMood.personalityMoodDict[i][kvp.Key] = new AnimalMood.MoodArray();
            }
        }
        // EditorUtility.SetDirty(animalMood);
        // AssetDatabase.SaveAssets();
    }
    public void defineAnimalDictionary() {
        animalDictionary["Llama"] = animalList[0];
        animalDictionary["Goose"] = animalList[1];
        animalDictionary["Rabbit"] = animalList[2];
        animalDictionary["Pig"] = animalList[3];
        animalDictionary["Sheep"] = animalList[4];
        animalDictionary["Fish"] = animalList[5];
    }
    public void SpawnAll() {
        foreach (KeyValuePair<int, Animal> kvp in curAnimals.animalDict) {
            // We use StartsWith to compare scene names because some animal.scenes (like if they are in a barn)
            // have scene names of the form <actualSceneName>#<someUniqueIdentifier>
            // kvp.Value.home = "Barn#0";
            if (kvp.Value.age >= 0 && !kvp.Value.characterOwned) {
                var sceneName = SceneManager.GetActiveScene().name;
                var sceneType = (Loader.Scene) Enum.Parse(typeof(Loader.Scene), sceneName);
                // if (kvp.Value.scene != "MainScene") {
                //     kvp.Value.scene = "Barn#0";
                // }
                if (kvp.Value.follow) {
                    kvp.Value.scene = sceneName;
                }
                var animalScene = kvp.Value.scene;
                // If the current scene is the Barn, then we need to check virtual barn
                if (sceneType == Loader.Scene.Barn) {
                    if (kvp.Value.follow) {
                        kvp.Value.scene = sceneName + '#' + placedBuildings.buildingEnteredIdx;
                        animalScene = kvp.Value.scene;
                    }
                    // animalScene should be of the form Barn1#<id of building>
                    if (animalScene.StartsWith(sceneName) &&
                        BuildingController.BuildingIdFromBuildingSceneName(animalScene) ==
                        placedBuildings.buildingEnteredIdx) {
                        Spawn(kvp.Value);
                    }
                } else if (sceneName == animalScene) {
                    Spawn(kvp.Value);
                }
            } else {
                kvp.Value.animalColors = animalColors;
            }
        }
    }
    public void playerWalkAnimal() {
        player.dailyWalk += 1;
    }
    public void playerTalkCharacter(int id) {
        player.dailyTalk += 1;
        player.dailyTalkedTo.Add(id);
    }
    public void playerGiftCharacter(int id) {
        player.dailyGiftCharacter += 1;
        player.dailyGiftedTo.Add(id);
    }
    public void playerGiftAnimal() {
        player.dailyGiftAnimal += 1;
    }
    public void setAnimalImage(GameObject animalImage, Animal animalTrait) {
        GameObject instance = animalImage;
        Transform instanceTrans = instance.transform;
        GameObject animal = animalDictionary[animalTrait.type];
        Transform animalTrans = animal.transform;
        instance.GetComponent<Image>().sprite = animal.GetComponent<SpriteRenderer>().sprite;
        //tail socket
        Transform instanceChildTrans = instanceTrans.Find("TailSocket");
        Transform animalChildTrans = animalTrans.Find("TailSocket");
        instanceChildTrans.gameObject.GetComponent<Image>().sprite = animalChildTrans.gameObject.GetComponent<SpriteRenderer>().sprite;
        //eyes socket
        instanceChildTrans = instanceTrans.Find("EyesSocket");
        animalChildTrans = animalTrans.Find("EyesSocket");
        instanceChildTrans.gameObject.GetComponent<Image>().sprite = animalChildTrans.gameObject.GetComponent<SpriteRenderer>().sprite;
        //star socket
        instanceChildTrans = instanceTrans.Find("StarSocket");
        animalChildTrans = animalTrans.Find("StarSocket");
        instanceChildTrans.gameObject.GetComponent<Image>().sprite = animalChildTrans.gameObject.GetComponent<SpriteRenderer>().sprite;
        //dots socket
        instanceChildTrans = instanceTrans.Find("DotsSocket");
        animalChildTrans = animalTrans.Find("DotsSocket");
        instanceChildTrans.gameObject.GetComponent<Image>().sprite = animalChildTrans.gameObject.GetComponent<SpriteRenderer>().sprite;
        //face socket
        instanceChildTrans = instanceTrans.Find("FaceSocket");
        animalChildTrans = animalTrans.Find("FaceSocket");
        instanceChildTrans.gameObject.GetComponent<Image>().sprite = animalChildTrans.gameObject.GetComponent<SpriteRenderer>().sprite;
        //Legs socket
        instanceChildTrans = instanceTrans.Find("LegsSocket");
        animalChildTrans = animalTrans.Find("LegsSocket");
        instanceChildTrans.gameObject.GetComponent<Image>().sprite = animalChildTrans.gameObject.GetComponent<SpriteRenderer>().sprite;
        //Back socket
        instanceChildTrans = instanceTrans.Find("BackSocket");
        animalChildTrans = animalTrans.Find("BackSocket");
        instanceChildTrans.gameObject.GetComponent<Image>().sprite = animalChildTrans.gameObject.GetComponent<SpriteRenderer>().sprite;
        //Ears socket
        instanceChildTrans = instanceTrans.Find("EarsSocket");
        animalChildTrans = animalTrans.Find("EarsSocket");
        instanceChildTrans.gameObject.GetComponent<Image>().sprite = animalChildTrans.gameObject.GetComponent<SpriteRenderer>().sprite;
    }
    public Transform findParent(int id) {
        return animalGameObject.GetComponent<AnimalList>().findAnimal(id);
    }
    public void Spawn(Animal a) {
        if (!animalDictionary.ContainsKey(a.type)) {
            defineAnimalDictionary();
        }
        GameObject instance = GameObject.Instantiate(animalDictionary[a.type]) as GameObject;
        if (a.type == "Fish") {
            instance.GetComponent<FishScript>().water = water;
        }
        instance.GetComponent<GenericAnimal>().animalModal = animalModal;
        // instance.GetComponent<GenericAnimal>().cameraObject = cameraGameObject;

        instance.GetComponent<GenericAnimal>().createAnimal(a);
        instance.GetComponent<GenericAnimal>().updateAnimal();
        if (a.personality != null) {
            instance.GetComponent<GenericAnimal>().giftMoods = animalMood.personalityMoodDict[a.personality.id][a.type].array;
        }
        instance.GetComponent<GenericAnimal>().reactions = animalMood.reactions;
        instance.GetComponent<GenericAnimal>().stop = false;
        instance.GetComponent<GenericAnimal>().moveSpeed = 2;
        instance.GetComponent<GenericAnimal>().spawnAnimal = gameObject;
        // instance.GetComponent<GenericAnimal>().animalTrait.spawnAnimal = gameObject;
        instance.GetComponent<GenericAnimal>().playerInventory = playerInventory;
        instance.GetComponent<GenericAnimal>().animalTrait.animalColors = animalColors;
        instance.GetComponent<GenericAnimal>().curAnimals = curAnimals;
        instance.GetComponent<GenericAnimal>().animalList = animalGameObject;
        //for breed identification post update
        Transform trans = instance.transform;
        if (a.age <= 1) {
            Vector3 scaleChange = new Vector3(0.45f, 0.45f, 0);
            trans.localScale = scaleChange;
        }
        if (a.age == 2) {
            Vector3 scaleChange = new Vector3(0.5f, 0.5f, 0);
            trans.localScale = scaleChange;
        }
        if (a.age == 3) {
            Vector3 scaleChange = new Vector3(0.6f, 0.6f, 0);
            trans.localScale = scaleChange;
        }
        if (a.age == 4) {
            Vector3 scaleChange = new Vector3(0.7f, 0.7f, 0);
            trans.localScale = scaleChange;
        }
        if (a.age <= 4 && !a.characterOwned) {
            instance.GetComponent<GenericAnimal>().moveSpeed *= 2;
            instance.GetComponent<GenericAnimal>().parent = findParent(a.momId);
            if (!instance.GetComponent<GenericAnimal>().parent) {
                instance.GetComponent<GenericAnimal>().parent = findParent(a.dadId);
            }
        }
        if (a.age > 60) {
            instance.GetComponent<GenericAnimal>().moveSpeed /= 3;
        }
        // Debug.Log(a.coloring["body"]);
        SpriteRenderer sprite = instance.GetComponent<SpriteRenderer>();
        sprite.color = animalColors.colorDictionary[a.coloring.body].color;

        Transform childTrans = trans.Find("TailSocket");
        sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
        sprite.color = animalColors.colorDictionary[a.coloring.tail].color;

        childTrans = trans.Find("StarSocket");
        if (animalColors.colorDictionary[a.coloring.star].ColorName == "Invisible") {
            childTrans.gameObject.SetActive(false);
        } else {
            childTrans.gameObject.GetComponent<SpriteRenderer>().color = animalColors.colorDictionary[a.coloring.star].color;
        }

        childTrans = trans.Find("DotsSocket");
        if (animalColors.colorDictionary[a.coloring.dots].ColorName == "Invisible") {
            childTrans.gameObject.SetActive(false);
        } else {
            sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
            sprite.color = animalColors.colorDictionary[a.coloring.dots].color;
        }

        childTrans = trans.Find("EyesSocket");
        sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
        sprite.color = animalColors.colorDictionary[a.coloring.eyes].color;

        childTrans = trans.Find("FaceSocket");
        if (animalColors.colorDictionary[a.coloring.face].ColorName == "Invisible") {
            childTrans.gameObject.SetActive(false);
        } else {
            sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
            sprite.color = animalColors.colorDictionary[a.coloring.face].color;
        }

        childTrans = trans.Find("BackSocket");
        if (animalColors.colorDictionary[a.coloring.back].ColorName == "Invisible") {
            childTrans.gameObject.SetActive(false);
        } else {
            sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
            sprite.color = animalColors.colorDictionary[a.coloring.back].color;
        }

        childTrans = trans.Find("EarsSocket");
        if (animalColors.colorDictionary[a.coloring.ears].ColorName == "Invisible") {
            childTrans.gameObject.SetActive(false);
        } else {
            sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
            sprite.color = animalColors.colorDictionary[a.coloring.ears].color;
        }

        childTrans = trans.Find("LegsSocket");
        if (animalColors.colorDictionary[a.coloring.legs].ColorName == "Invisible") {
            childTrans.gameObject.SetActive(false);
        } else {
            sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
            sprite.color = animalColors.colorDictionary[a.coloring.legs].color;
        }
        animalGameObject.GetComponent<AnimalList>().addExistingAnimal(instance);
    }

    public void SpawnCharAnimal(Animal a, Transform character) {
        defineAnimalDictionary();
        GameObject instance = GameObject.Instantiate(animalDictionary[a.type]) as GameObject;
        instance.GetComponent<GenericAnimal>().animalModal = animalModal;
        // instance.GetComponent<GenericAnimal>().cameraObject = cameraGameObject;
        instance.GetComponent<GenericAnimal>().createAnimal(a);
        instance.GetComponent<GenericAnimal>().updateAnimal();
        if (a.personality != null) {
            instance.GetComponent<GenericAnimal>().giftMoods = animalMood.personalityMoodDict[a.personality.id][a.type].array;
        }
        instance.GetComponent<GenericAnimal>().reactions = animalMood.reactions;
        instance.GetComponent<GenericAnimal>().moveSpeed = 3;
        instance.GetComponent<GenericAnimal>().animalTrait.location = character.position - new Vector3(1, 0, 0);
        //make double sure it's where we want it
        instance.transform.position = character.position - new Vector3(1, 0, 0);
        instance.GetComponent<GenericAnimal>().target = character;
        instance.GetComponent<GenericAnimal>().spawnAnimal = gameObject;
        instance.GetComponent<GenericAnimal>().animalTrait.follow = true;
        // Configure the spawned animal to follow the NPC
        instance.GetComponent<GenericAnimal>().startAStar(character);
        // instance.GetComponent<GenericAnimal>().animalTrait.spawnAnimal = gameObject;
        instance.GetComponent<GenericAnimal>().playerInventory = playerInventory;
        instance.GetComponent<GenericAnimal>().animalTrait.animalColors = animalColors;
        instance.GetComponent<GenericAnimal>().curAnimals = curAnimals;
        instance.GetComponent<GenericAnimal>().animalList = animalGameObject;
        //for breed identification post update
        Transform trans = instance.transform;
        if (a.age <= 1) {
            Vector3 scaleChange = new Vector3(0.45f, 0.45f, 0);
            trans.localScale = scaleChange;
        }
        if (a.age == 2) {
            Vector3 scaleChange = new Vector3(0.5f, 0.5f, 0);
            trans.localScale = scaleChange;
        }
        if (a.age == 3) {
            Vector3 scaleChange = new Vector3(0.6f, 0.6f, 0);
            trans.localScale = scaleChange;
        }
        if (a.age == 4) {
            Vector3 scaleChange = new Vector3(0.7f, 0.7f, 0);
            trans.localScale = scaleChange;
        }
        // Debug.Log(a.coloring["body"]);
        SpriteRenderer sprite = instance.GetComponent<SpriteRenderer>();
        sprite.color = animalColors.colorDictionary[a.coloring.body].color;

        Transform childTrans = trans.Find("TailSocket");
        sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
        sprite.color = animalColors.colorDictionary[a.coloring.tail].color;

        childTrans = trans.Find("StarSocket");
        if (animalColors.colorDictionary[a.coloring.star].ColorName == "Invisible") {
            childTrans.gameObject.SetActive(false);
        } else {
            childTrans.gameObject.GetComponent<SpriteRenderer>().color = animalColors.colorDictionary[a.coloring.star].color;
        }

        childTrans = trans.Find("DotsSocket");
        if (animalColors.colorDictionary[a.coloring.dots].ColorName == "Invisible") {
            childTrans.gameObject.SetActive(false);
        } else {
            sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
            sprite.color = animalColors.colorDictionary[a.coloring.dots].color;
        }

        childTrans = trans.Find("EyesSocket");
        sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
        sprite.color = animalColors.colorDictionary[a.coloring.eyes].color;

        childTrans = trans.Find("FaceSocket");
        if (animalColors.colorDictionary[a.coloring.face].ColorName == "Invisible") {
            childTrans.gameObject.SetActive(false);
        } else {
            sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
            sprite.color = animalColors.colorDictionary[a.coloring.face].color;
        }

        childTrans = trans.Find("BackSocket");
        if (animalColors.colorDictionary[a.coloring.back].ColorName == "Invisible") {
            childTrans.gameObject.SetActive(false);
        } else {
            sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
            sprite.color = animalColors.colorDictionary[a.coloring.back].color;
        }

        childTrans = trans.Find("EarsSocket");
        if (animalColors.colorDictionary[a.coloring.ears].ColorName == "Invisible") {
            childTrans.gameObject.SetActive(false);
        } else {
            sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
            sprite.color = animalColors.colorDictionary[a.coloring.ears].color;
        }

        childTrans = trans.Find("LegsSocket");
        if (animalColors.colorDictionary[a.coloring.legs].ColorName == "Invisible") {
            childTrans.gameObject.SetActive(false);
        } else {
            sprite = childTrans.gameObject.GetComponent<SpriteRenderer>();
            sprite.color = animalColors.colorDictionary[a.coloring.legs].color;
        }
        animalGameObject.GetComponent<AnimalList>().addExistingAnimal(instance);
    }
}
