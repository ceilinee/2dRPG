using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class IslandGeneration : CustomMonoBehaviour {
    //using Conways Game Of Life
    //Youtube tutorial: https://www.youtube.com/watch?v=xNqqfABXTNQ
    [Range(0, 100)]
    public int iniChance;
    [Range(1, 8)]
    public int birthLimit;
    [Range(1, 8)]
    public int deathLimit;

    [Range(1, 10)]
    public int numR;
    private Transform Player;
    private Vector2 playerLocation;
    public CameraMovement cam;
    private int count = 0;
    private int[,] terrainMap;
    private int[,] forestMap;

    public Vector3Int tmpSize;
    public Tilemap topMap;
    public Tilemap botMap;
    public Tilemap grassMap;
    public Tilemap collisionMap;
    public AnimatedTile collisionTile;
    public bool random;
    public bool beach2;
    public GameObject returnBoat;
    public RuleTile beachTile;
    public RuleTile beachTile2;
    public RuleTile grassTile;
    public AnimatedTile[] waterTile;
    private SpawnObject spawnObject;
    private SpawnWildAnimal spawnWildAnimal;

    int width;
    int height;
    //generate forest
    public bool generateForest(int nu) {
        clearForestMap(false);
        width = tmpSize.x;
        height = tmpSize.y;

        if (forestMap == null) {
            forestMap = new int[width, height];
            initPos(forestMap, true);
        }


        for (int i = 0; i < nu; i++) {
            forestMap = genTilePos(forestMap);
        }

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (forestMap[x, y] == 1 && terrainMap[x, y] == 1) {
                    grassMap.SetTile(ReturnProperPosition(x, y, 0), grassTile);
                }
            }
        }
        grassMap.gameObject.SetActive(true);
        return true;
    }
    //confirm whether valid tree planting spot
    public bool isValidTreePlantingSpot(int x, int y) {
        return beach2 ? terrainMap[x, y] == 1 : forestMap[x, y] == 1 && terrainMap[x, y] == 1;
    }
    //generate map for the beach
    public bool generateBeach(int nu) {
        clearMap(false);
        width = tmpSize.x;
        height = tmpSize.y;
        // Vector2 locationOrigin = new Vector2(0, 0);
        // topMap.transform.position = locationOrigin;
        // botMap.transform.position = locationOrigin;
        // collisionMap.transform.position = locationOrigin;
        // Player.transform.position = new Vector2(width / 2, height / 2);

        if (terrainMap == null) {
            terrainMap = new int[width, height];
            initPos(terrainMap);
        }


        for (int i = 0; i < nu; i++) {
            terrainMap = genTilePos(terrainMap);
        }

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (terrainMap[x, y] == 1) {
                    botMap.SetTile(ReturnProperPosition(x, y, 0), beach2 ? beachTile2 : beachTile);
                } else {
                    topMap.SetTile(ReturnProperPosition(x, y, 0), waterTile[Random.Range(0, waterTile.Length)]);
                }
            }
        }
        for (int x = 0; x < width; x++) {
            collisionMap.SetTile(ReturnProperPosition(x, 0, 0), collisionTile);
            collisionMap.SetTile(ReturnProperPosition(x, height - 1, 0), collisionTile);
        }
        for (int y = 0; y < height; y++) {
            collisionMap.SetTile(ReturnProperPosition(0, y, 0), collisionTile);
            collisionMap.SetTile(ReturnProperPosition(width - 1, y, 0), collisionTile);
        }
        cam.maxPosition = ReturnProperPositionV2(width - 15, height - 6);
        cam.minPosition = ReturnProperPositionV2(0 + 15, 0 + 6);
        return true;
    }
    public bool generateGrassBeach(int nu) {
        width = tmpSize.x;
        height = tmpSize.y;
        // Vector2 locationOrigin = new Vector2(0, 0);
        // topMap.transform.position = locationOrigin;
        // botMap.transform.position = locationOrigin;
        // collisionMap.transform.position = locationOrigin;
        // Player.transform.position = new Vector2(width / 2, height / 2);

        if (terrainMap == null) {
            terrainMap = new int[width, height];
        }


        for (int i = 0; i < nu * 5; i++) {
            terrainMap = drawRectangle(terrainMap);
        }
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (terrainMap[x, y] == 1) {
                    botMap.SetTile(ReturnProperPosition(x, y, 0), beach2 ? beachTile2 : beachTile);
                } else {
                    topMap.SetTile(ReturnProperPosition(x, y, 0), waterTile[Random.Range(0, waterTile.Length)]);
                }
            }
        }
        cam.maxPosition = ReturnProperPositionV2(width - 15, height - 6);
        cam.minPosition = ReturnProperPositionV2(0 + 15, 0 + 6);
        return true;
    }
    public int[,] drawRectangle(int[,] map) {
        int rectWidth = Random.Range(2, width / 2);
        int rectHeight = Random.Range(2, height / 2);
        Vector2 pickPoint = new Vector2(Random.Range(1, width), Random.Range(1, height));
        for (int i = 0; i < rectWidth; i++) {
            for (int j = 0; j < rectHeight; j++) {
                if (pickPoint.x + i >= width - 1 || pickPoint.y + j >= height - 1) {
                    break;
                }
                map[(int) pickPoint.x + i, (int) pickPoint.y + j] = 1;
            }
        }
        Debug.Log(map);
        return map;
    }

    public Vector3Int ReturnProperPosition(int x, int y, int z) {
        return new Vector3Int(-x + width / 2, -y + height / 2, z);
    }
    public Vector2 ReturnProperPositionV2(int x, int y) {
        return new Vector2(-(-x + width / 2), -(-y + height / 2));
    }
    public void initPos(int[,] map, bool forest = false) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                map[x, y] = forest ?
                Random.Range(1, 101) < (1 / (Mathf.Sqrt(Vector2.Distance(new Vector2(x, y), new Vector2(width / 2, width / 2)))) * iniChance) ? 1 : 0
                :
                Random.Range(1, 101) < (1 / Mathf.Sqrt(Vector2.Distance(new Vector2(x, y), new Vector2(width / 2, width / 2))) * iniChance) ? 1 : 0;
            }
        }
    }

    public int[,] genTilePos(int[,] oldMap, bool forest = false) {
        int[,] newMap = new int[width, height];
        int neighb;
        BoundsInt myB = new BoundsInt(-1, -1, 0, 3, 3, 1);


        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                neighb = 0;
                foreach (var b in myB.allPositionsWithin) {
                    if (b.x == 0 && b.y == 0) continue;
                    if (x + b.x >= 0 && x + b.x < width && y + b.y >= 0 && y + b.y < height) {
                        neighb += oldMap[x + b.x, y + b.y];
                    }
                    // else {
                    //     neighb++;
                    // }
                }

                if (oldMap[x, y] == 1) {
                    if (neighb < deathLimit) newMap[x, y] = 0;

                    else {
                        newMap[x, y] = 1;

                    }
                }

                if (oldMap[x, y] == 0) {
                    if (neighb > birthLimit) newMap[x, y] = 1;

                    else {
                        newMap[x, y] = 0;
                    }
                }

            }

        }

        StartCoroutine(ColliderWait());

        return newMap;

    }

    private IEnumerator ColliderWait() {
        yield return new WaitForEndOfFrame(); // need this!

        topMap.GetComponent<CompositeCollider2D>().GenerateGeometry();
        collisionMap.GetComponent<CompositeCollider2D>().GenerateGeometry();

        yield return new WaitForEndOfFrame();
    }
    private IEnumerator GenerationWait() {
        bool forestResult = false;
        bool beachResult = false;
        beachResult = beach2 ? generateGrassBeach(numR) : generateBeach(numR);
        grassMap.gameObject.SetActive(false);
        forestResult = beach2 ? true : generateForest(numR);
        while (!forestResult || !beachResult) {
            yield return null;
        }
        PlantTrees();
        GenerateAnimal();
        GenerateObjects();
        SetReturnBoat();
    }
    public void SetReturnBoat() {
        returnBoat.transform.position = GetSmallestPoint(terrainMap);
        returnBoat.SetActive(true);
    }
    public Vector2 GetSmallestPoint(int[,] map) {
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (map[x, y] == 1) {
                    return ReturnProperPositionV2(x + 10, y + 10);
                }
            }
        }
        return new Vector2(0, 0);
    }
    public void GenerateObjects() {
        spawnObject.Spawn();
    }
    //Generate wild animals
    public void GenerateAnimal() {
        int animals = Random.Range(0, 10);
        for (int i = 0; i < animals; i++) {
            // attempt to find a spot where a tree can be planted
            int x = Random.Range(width / 8, 7 * width / 8);
            int y = Random.Range(height / 8, 7 * height / 8);
            spawnWildAnimal.generateAnimal(ReturnProperPositionV2(x, y));
        }
    }
    public void PlantTrees() {
        int trees = Random.Range(0, 10);
        int attempts = 3;
        for (int i = 0; i < trees; i++) {
            // attempt to find a spot where a tree can be planted
            for (int j = 0; j < attempts; j++) {
                int x = Random.Range(width / 4, 3 * width / 4);
                int y = Random.Range(height / 4, 3 * height / 4);
                if (isValidTreePlantingSpot(x, y)) {
                    spawnObject.generateTree(ReturnProperPositionV2(x, y));
                }
            }
        }
    }
    void Start() {
        // if (!beach2) {
        //     beach2 = Random.Range(0, 2) >= 1 ? true : false;
        // }
        Player = centralController.Get("Player").transform;
        spawnObject = centralController.Get("SpawnObjects").GetComponent<SpawnObject>();
        spawnWildAnimal = centralController.Get("WildAnimalSpawner").GetComponent<SpawnWildAnimal>();
        StartCoroutine(GenerationWait());
        tmpSize = random ? new Vector3Int(Random.Range(80, 150), Random.Range(80, 150), 0) : tmpSize;
    }
    // void Update() {

    //     if (Input.GetMouseButtonDown(0)) {
    //         doSim(numR);
    //     }


    //     if (Input.GetMouseButtonDown(1)) {
    //         clearMap(true);
    //     }



    //     if (Input.GetMouseButton(2)) {
    //         SaveAssetMap();
    //         count++;
    //     }
    // }


    // public void SaveAssetMap() {
    //     string saveName = "tmapXY_" + count;
    //     var mf = GameObject.Find("Grid");

    //     if (mf) {
    //         var savePath = "Assets/" + saveName + ".prefab";
    //         if (PrefabUtility.CreatePrefab(savePath, mf)) {
    //             EditorUtility.DisplayDialog("Tilemap saved", "Your Tilemap was saved under" + savePath, "Continue");
    //         } else {
    //             EditorUtility.DisplayDialog("Tilemap NOT saved", "An ERROR occured while trying to saveTilemap under" + savePath, "Continue");
    //         }

    //     }
    // }
    public void clearForestMap(bool complete) {
        grassMap.ClearAllTiles();
        if (complete) {
            terrainMap = null;
        }
    }
    public void clearMap(bool complete) {
        topMap.ClearAllTiles();
        botMap.ClearAllTiles();
        if (complete) {
            terrainMap = null;
        }
    }
}
