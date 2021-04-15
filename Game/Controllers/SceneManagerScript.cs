using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public GameObject spawnAnimals;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    // void OnDisable () {
    //    Debug.Log("disable");
    // }
    // void OnEnable()
    // {/
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    // }
    // void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     Debug.Log(spawnAnimals);
    //     if(scene.buildIndex == 0){
    //       spawnAnimals.GetComponent<SpawnAnimal>().SpawnAll();
    //     }
    // }

}
