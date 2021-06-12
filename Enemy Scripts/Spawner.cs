using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    // Start is called before the first frame update
    public Transform[] spawnPoints;
    public GameObject[] monsters;
    int randomSpawnPoint, randomMonster;
    public bool spawnAllowed;
    void Start() {
        // spawnAllowed = true;
        if (spawnAllowed) {
            InvokeRepeating("SpawnAGoose", 0f, 2f);
        }
    }

    public void SpawnMultipleGeese(int count) {
        for (int i = 0; i < count; i++) {
            SpawnAGoose();
        }
    }
    public void SpawnMultipleGeese(int count, Vector2 pos) {
        for (int i = 0; i < count; i++) {
            SpawnAGoose(pos);
        }
    }
    public void SpawnAGoose() {
        randomSpawnPoint = Random.Range(0, spawnPoints.Length);
        SpawnAGoose(spawnPoints[randomSpawnPoint].position);
    }
    public void SpawnAGoose(Vector2 pos, int monster = -1) {
        randomMonster = monster == -1 ? Random.Range(0, monsters.Length) : monster;
        Instantiate(monsters[randomMonster], pos, Quaternion.identity);
    }
}
