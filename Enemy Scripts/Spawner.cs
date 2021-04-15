using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] spawnPoints;
    public GameObject[] monsters;
    int randomSpawnPoint, randomMonster;
    int count;
    public static bool spawnAllowed;
    void Start()
    {
        count = 0;
        spawnAllowed = true;
        InvokeRepeating("SpawnAMonster", 0f, 2f);
    }

    void SpawnAMonster(){
        if(spawnAllowed){
            randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            randomMonster = Random.Range(0, monsters.Length);
            count++;
            Instantiate(monsters[randomMonster], spawnPoints[randomSpawnPoint].position, Quaternion.identity);
        }
        if(count > 5){
           CancelInvoke("SpawnAMonster");
        }
    }
}
