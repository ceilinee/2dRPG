﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEngine.SceneManagement;
public enum PreviousDirection {
    LEFT,
    RIGHT,
    UP,
    DOWN
}
public class ForestGeneration : CustomMonoBehaviour {
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
    private int[,] terrainMap;
    private int[,] forestMap;

    public Vector3Int tmpSize;
    public Tilemap topMap;
    public Tilemap botMap;
    public Tilemap grassMap;
    public Tilemap collisionMap;
    public RuleTile collisionTile;
    public bool random;
    public bool maze = true;
    public Forest forest;
    public GameObject returnBoat;
    public RuleTile beachTile;
    public RuleTile beachTile2;
    public RuleTile trainTrack;
    public RuleTile grassTile;
    public AnimatedTile[] waterTile;
    private SpawnObject spawnObject;
    // public Item flag;
    private SpawnWildAnimal spawnWildAnimal;
    private DialogueManager dialogueManager;
    public GameObject ranger;
    public Dialogue rangerDialogue;
    private DisplayArea displayArea;

    int width;
    int height;
    public void StartForest() {
        Debug.Log("Forest Width: " + forest.width);
        maze = forest.maze;
        Player = centralController.Get("Player").transform;
        dialogueManager = centralController.Get("DialogueManager").GetComponent<DialogueManager>();
        spawnObject = centralController.Get("SpawnObjects").GetComponent<SpawnObject>();
        spawnWildAnimal = centralController.Get("WildAnimalSpawner").GetComponent<SpawnWildAnimal>();
        displayArea = centralController.Get("DisplayArea").GetComponent<DisplayArea>();
        tmpSize = new Vector3Int(forest.width, forest.height, 0);
        StartCoroutine(GenerationWait());
        if (forest.level == 0) {
            dialogueManager.startDialog(ranger, rangerDialogue);
        }
        displayArea.startAlert("Forest - Area " + forest.level);
    }

    //confirm whether valid tree planting spot
    public bool isValidTreePlantingSpot(int value, int x, int y, int size) {
        return maze ? AreaIs1(value, terrainMap, x, y, size)
        : AreaIs1(value, forestMap, x, y, size)
        && AreaIs1(value, terrainMap, x, y, size);
    }
    public bool AreaIs1(int value, int[,] map, int x, int y, int size) {
        for (int i = x - (size / 2); i < x + 1 + (size / 2); i++) {
            for (int j = y - (size / 2); j < y + 1 + (size / 2); j++) {
                if (i >= map.GetLength(0) || j >= map.GetLength(1) || i < 0 || j < 0) {
                    return false;
                }
                if (map[i, j] != value) {
                    return false;
                }
            }
        }
        return true;
    }
    //generate map for the beach
    public bool generateSwamp(int nu) {
        clearMap(false);
        width = tmpSize.x;
        height = tmpSize.y;

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
                    botMap.SetTile(ReturnProperPosition(x, y, 0), maze ? beachTile2 : beachTile);
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
    public bool generateMaze(int nu) {
        width = tmpSize.x;
        height = tmpSize.y;
        int starCount = (forest.level + 1) * 2;
        if (terrainMap == null) {
            terrainMap = new int[width / 2, height / 2];
        }

        terrainMap = drawMaze(terrainMap);
        terrainMap = doubleMap(terrainMap);
        terrainMap = drawRectangle(terrainMap, 3, height - 2, true);
        for (int x = 0; x <= width; x++) {
            for (int y = 0; y <= height; y++) {
                grassMap.SetTile(ReturnProperPosition(x, y, 0), grassTile);
            }
        }
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (terrainMap[x, y] == 0) {
                    collisionMap.SetTile(ReturnProperPosition(x, y, 0), collisionTile);
                }
            }
        }
        for (int x = 0; x < width; x++) {
            collisionMap.SetTile(ReturnProperPosition(x, 0, 0), collisionTile);
            collisionMap.SetTile(ReturnProperPosition(x, height, 0), collisionTile);
        }
        for (int y = 0; y < height; y++) {
            collisionMap.SetTile(ReturnProperPosition(0, y, 0), collisionTile);
            collisionMap.SetTile(ReturnProperPosition(width, y, 0), collisionTile);
        }
        //generate train tracks
        for (int x = 0; x < width; x++) {
            for (int y = 1; y < 3; y++) {
                topMap.SetTile(ReturnProperPosition(x, y, 0), trainTrack);
            }
        }
        cam.maxPosition = ReturnProperPositionV2(width - 12, height - 6);
        cam.minPosition = ReturnProperPositionV2(0 + 14, 0 + 8);
        return true;
    }
    public int[,] doubleMap(int[,] map) {
        int[,] newMap = new int[map.GetLength(0) * 2, map.GetLength(1) * 2];
        setZeroes(newMap);
        for (int x = 0; x < map.GetLength(0); x++) {
            for (int y = 0; y < map.GetLength(1); y++) {
                newMap[(x * 2), y] = map[x, y];
                newMap[(x * 2) + 1, y] = map[x, y];
            }
        }
        for (int x = 0; x < newMap.GetLength(0); x++) {
            for (int y = map.GetLength(1) - 1; y >= 0; y--) {
                newMap[x, (y * 2)] = newMap[x, y];
                newMap[x, (y * 2) + 1] = newMap[x, y];
            }
        }
        return newMap;
    }
    public int[,] setZeroes(int[,] map) {
        for (int x = 0; x < map.GetLength(0); x++) {
            for (int y = 0; y < map.GetLength(1); y++) {
                map[x, y] = 0;
            }
        }
        return map;
    }
    public int[,] drawMaze(int[,] map) {
        //set all as zero
        map = setZeroes(map);
        //set entrance and exit
        Vector2 entrance = new Vector2(1, 1);
        spawnObject.generatePrefab(ReturnProperPositionV2((int) (entrance.x * 2) + 1, (int) (entrance.y * 2) + 1), "HOME");
        Vector2 exit = new Vector2(map.GetLength(0) - 1, Random.Range(1, map.GetLength(1) - 1));
        spawnObject.generatePrefab(ReturnProperPositionV2((int) (exit.x * 2) + 2, (int) (exit.y * 2) + 1), "LLAMASTATUE");
        int curStarCount = -2;
        map = makeRealPath(exit, map, (int) entrance.x, (int) entrance.y);
        for (int i = 0; i < map.GetLength(1); i++) {
            int random = Random.Range(0, 10);
            if (random == 0) {
                Vector2 randomEntrance = new Vector2(1, Random.Range(1, map.GetLength(1) - 1));
                Vector2 randomExit = new Vector2(Random.Range(6 * map.GetLength(0) / 8, map.GetLength(0) - 1), Random.Range(0, map.GetLength(1)));
                map = makeRealPath(randomExit, map, (int) randomEntrance.x, (int) randomEntrance.y);
                if (curStarCount < forest.starCount) {
                    spawnObject.generatePrefab(ReturnProperPositionV2((int) (randomExit.x * 2) + 1, (int) (randomExit.y * 2) + 1), "STAR");
                    curStarCount++;
                }
            }
        }
        for (int i = 0; i < forest.starCount - curStarCount; i++) {
            bool generated = false;
            while (generated == false) {
                int x = Random.Range(1, width - 1);
                int y = Random.Range(1, height - 1);
                if (isValidTreePlantingSpot(1, x, y, 1)) {
                    generated = true;
                    spawnObject.generatePrefab(ReturnProperPositionV2((int) (x * 2) + 1, (int) (y * 2) + 1), "STAR");
                }
            }
        }
        return map;
    }

    int[,] makeRealPath(Vector2 exit, int[,] map, int x, int y, PreviousDirection previousDirection = PreviousDirection.RIGHT) {
        //if at end return, should never reach this
        if (x >= map.GetLength(0) || y >= map.GetLength(1) || x < 0 || y < 0) {
            return map;
        }
        //set self as path
        map[x, y] = 1;
        //if reached goal
        if (x == exit.x && y == exit.y) {
            return map;
        }
        //reached end, move up or down
        if (x == exit.x) {
            if (y > exit.y) {
                makeRealPath(exit, map, x, y - 1, PreviousDirection.DOWN);
            } else if (y < exit.y) {
                makeRealPath(exit, map, x, y + 1, PreviousDirection.UP);
            }
        }
        //reached limits
        else if (x == 0) {
            makeRealPath(exit, map, x + 1, y, PreviousDirection.RIGHT);
        } else if (y == 0) {
            makeRealPath(exit, map, x, y + 1, PreviousDirection.UP);
        } else if (y == map.GetLength(1) - 1) {
            makeRealPath(exit, map, x, y - 1, PreviousDirection.DOWN);
        } else {
            int random = Random.Range(0, 11);
            // if (previousDirection == PreviousDirection.DOWN && Random.Range(0, 2) >= 1) {
            //     random = 1;
            // } else if (previousDirection == PreviousDirection.UP && Random.Range(0, 2) >= 1) {
            //     random = 6;
            // } else if (previousDirection == PreviousDirection.RIGHT && Random.Range(0, 4) >= 3) {
            //     random = 0;
            // }
            switch (random) {
                case 0:
                    makeRealPath(exit, map, x + 1, y, PreviousDirection.RIGHT);
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    makeRealPath(exit, map, x, y - 1, PreviousDirection.DOWN);
                    break;
                default:
                    makeRealPath(exit, map, x, y + 1, PreviousDirection.UP);
                    break;
            }
        }
        return map;
        // else random path

    }
    public int[,] drawRectangle(int[,] map, int x = -1, int y = -1, bool beginning = false) {
        int rectWidth = x == -1 ? Random.Range(2, width / 2) : x;
        int rectHeight = y == -1 ? Random.Range(2, height / 2) : y;
        // if draw empty area at start
        Vector2 pickPoint = beginning ? new Vector2(1, 1) : new Vector2(Random.Range(1, width), Random.Range(1, height));
        for (int i = 0; i < rectWidth; i++) {
            for (int j = 0; j < rectHeight; j++) {
                if (pickPoint.x + i >= width - 1 || pickPoint.y + j >= height - 1) {
                    break;
                }
                map[(int) pickPoint.x + i, (int) pickPoint.y + j] = 1;
            }
        }
        return map;
    }
    public Vector3Int ReturnProperPosition(int x, int y, int z) {
        return new Vector3Int(x - (width / 2), y - (width / 2), z);
    }
    public Vector2 ReturnProperPositionV2(int x, int y) {
        return new Vector2(x - (width / 2), y - (width / 2));
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
        bool beachResult = false;
        beachResult = maze ? generateMaze(numR) : generateSwamp(numR);
        while (!beachResult) {
            yield return null;
        }
        PlantTrees();
        GenerateAnimal();
        GenerateObjects();
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
        int attempts = 3;

        for (int i = 0; i < animals; i++) {
            for (int j = 0; j < attempts; j++) {
                // attempt to find a spot where a tree can be planted
                int x = Random.Range(width / 8, 7 * width / 8);
                int y = Random.Range(height / 8, 7 * height / 8);
                if (isValidTreePlantingSpot(1, x, y, 1)) {
                    spawnWildAnimal.generateAnimal(ReturnProperPositionV2(x, y));
                    break;
                }
            }
        }
    }
    public void PlantTrees() {
        int trees = Random.Range(0, width / 2);
        int attempts = 20;
        for (int i = 0; i < trees; i++) {
            // attempt to find a spot where a tree can be planted
            for (int j = 0; j < attempts; j++) {
                int x = Random.Range(0, width);
                int y = Random.Range(0, height);
                if (isValidTreePlantingSpot(0, x, y, 7)) {
                    spawnObject.generateTree(ReturnProperPositionV2(x, y));
                    spawnObject.generatePrefab(ReturnProperPositionV2(x + Random.Range(-4, 0), y + Random.Range(-4, 0)), "LAMP");
                    break;
                }
            }
        }
    }

    void OnEnable() {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        StartForest();
    }
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
