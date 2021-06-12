using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using System;

public class SceneTransition : CustomMonoBehaviour {
    // Scene information of the scene being transitioned to
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
    public bool confirm;
    public Confirmation confirmation;
    public VectorValue playerStorage;

    [SerializeField]
    private SceneInfo mainSceneInfo;

    [SerializeField]
    private SignalString playerWillSceneTransition;
    public class OnEntityWillEnterSceneTransitionEventArgs : EventArgs {
        public Collider2D entity;
    }
    private event EventHandler<OnEntityWillEnterSceneTransitionEventArgs> OnPetWillEnterSceneTransition;

    void Awake() {
        Assert.IsNotNull(playerWillSceneTransition,
            "This variable should be set through the inspector. Please drag SO PlayerWillSceneTransition over!");
    }
    protected virtual void Start() {
        gameSaveManager = GameObject.FindGameObjectsWithTag("save")[0];
        animalList = centralController.Get("AnimalList");
        confirmation = centralController.Get("Confirmation").GetComponent<Confirmation>();
    }

    public void PlayerTransition(SceneInfo newScene = null) {
        playerStorage.initialValue = newScene.entrance;
        gameSaveManager.GetComponent<GameSaveManager>().updateAnimalAndCharacter();
        // characterManager.GetComponent<CharacterManager>().updateCurCharacter();
        // animalList.GetComponent<AnimalList>().updateList();
        if (!player.dailyScenesVisited.Contains(newScene.id)) {
            player.dailyScenesVisited.Add(newScene.id);
        }
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Forest" && inForest) {
            if (newScene.sceneName == "Forest") {
                forest.LevelUp();
                playerStorage.initialValue = new Vector2(3 - (forest.width / 2), 6 - (forest.height / 2));
            }
        }
        // TODO: refactor this logic; be able query for mainscene dynamically
        if (scene.name == "MainScene") {
            mainSceneInfo.entrance = centralController.Get("Player").transform.position + new Vector3(0, -1, 0);
            mainSceneInfo.ForceSerialization();
            //entering forest
            if (newScene.sceneName == "GeeseMiniGame") {
                playerStorage.initialValue = new Vector2(8 - (newScene.width / 2), 0);
            }
            if (newScene.sceneName == "Forest") {
                StartCoroutine(waitUpdateForest());
                return;
            }
        }
        playerWillSceneTransition.Raise(newScene.sceneName);
        Loader.Load(sceneName: newScene.sceneName);
    }
    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(TagOfPlayer) && !other.isTrigger) {
            if (confirmation && confirm) {
                confirmation.initiateConfirmation(
                    "Are you sure you want to leave the " + SceneManager.GetActiveScene().name + "?",
                    () => PlayerTransition(sceneinfo),
                    () => { },
                    () => { }
                );
            } else {
                PlayerTransition(sceneinfo);
            }
        } else if (other.CompareTag("pet") && !other.isTrigger) {
            var animal = curAnimals.animalDict[other.gameObject.GetComponent<GenericAnimal>().animalTrait.id];
            animal.scene = sceneinfo.sceneName;
            animal.location = sceneinfo.entrance;
            animalList.GetComponent<AnimalList>().removeAnimal(other.gameObject.GetComponent<GenericAnimal>().animalTrait.id);

            // If we have any registered event listeners, then notify them
            OnPetWillEnterSceneTransition?.Invoke(
                this, new OnEntityWillEnterSceneTransitionEventArgs { entity = other });

            Destroy(other.gameObject);
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
        playerWillSceneTransition.Raise(sceneinfo.sceneName);
        Loader.Load(sceneName: sceneinfo.sceneName);
    }
    public void SetSceneInfo(SceneInfo sceneInfo) {
        // sceneInfo.entrance = GetComponent<BoxCollider2D>().transform.position;
        sceneinfo = sceneInfo;
    }

    public void AddPetWillEnterSceneTransitionListener(
        EventHandler<OnEntityWillEnterSceneTransitionEventArgs> handler) {
        OnPetWillEnterSceneTransition += handler;
    }
}
