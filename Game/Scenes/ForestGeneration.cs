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
    private AstarPath astarPath;
    private Spawner spawner;
    private GenerationSharedFunctions functions;
    int width;
    int height;
    public void SetWidthHeight() {
        width = tmpSize.x;
        height = tmpSize.y;
        functions = new GenerationSharedFunctions();
        functions.Initiate(width, height);
    }
    public void StartForest() {
        tmpSize = new Vector3Int(forest.width, forest.height, 0);
        SetWidthHeight();
        maze = forest.maze;
        Player = centralController.Get("Player").transform;
        dialogueManager = centralController.Get("DialogueManager").GetComponent<DialogueManager>();
        spawnObject = centralController.Get("SpawnObjects").GetComponent<SpawnObject>();
        spawnWildAnimal = centralController.Get("WildAnimalSpawner").GetComponent<SpawnWildAnimal>();
        spawner = centralController.Get("Spawner").GetComponent<Spawner>();
        astarPath = centralController.Get("A*").GetComponent<AstarPath>();
        displayArea = centralController.Get("DisplayArea").GetComponent<DisplayArea>();
        StartCoroutine(GenerationWait());
        if (forest.level == 0) {
            dialogueManager.startDialog(ranger, rangerDialogue);
        }
        displayArea.startAlert("Forest - Area " + forest.level);
    }

    public bool generateMaze(int nu) {
        width = tmpSize.x;
        height = tmpSize.y;
        int starCount = (forest.level + 1) * 2;
        if (terrainMap == null) {
            terrainMap = new int[width / 2, height / 2];
        }

        terrainMap = functions.drawMaze(terrainMap, spawnObject);
        terrainMap = functions.doubleMap(terrainMap);
        terrainMap = functions.drawRectangle(terrainMap, 3, height - 2, beginning: true);
        if (forest.campsite) {
            terrainMap = functions.drawRectangle(terrainMap, width / 4, height / 4, center: true);
            // terrainMap = drawRectangle(terrainMap, width / 6, height / 6, center: true, value: 2);
        }
        for (int x = 0; x <= width; x++) {
            for (int y = 0; y <= height; y++) {
                grassMap.SetTile(functions.ReturnProperPosition(x, y, 0), grassTile);
            }
        }
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (terrainMap[x, y] == 0) {
                    collisionMap.SetTile(functions.ReturnProperPosition(x, y, 0), collisionTile);
                }
                if (terrainMap[x, y] == 2) {
                    topMap.SetTile(functions.ReturnProperPosition(x, y, 0), beachTile);
                }
            }
        }
        for (int x = 0; x < width; x++) {
            collisionMap.SetTile(functions.ReturnProperPosition(x, 0, 0), collisionTile);
            collisionMap.SetTile(functions.ReturnProperPosition(x, height, 0), collisionTile);
        }
        for (int y = 0; y < height; y++) {
            collisionMap.SetTile(functions.ReturnProperPosition(0, y, 0), collisionTile);
            collisionMap.SetTile(functions.ReturnProperPosition(width, y, 0), collisionTile);
        }
        //generate train tracks
        for (int x = 0; x < width; x++) {
            for (int y = 1; y < 3; y++) {
                topMap.SetTile(functions.ReturnProperPosition(x, y, 0), trainTrack);
            }
        }
        cam.maxPosition = functions.ReturnProperPositionV2(width - 12, height - 6);
        cam.minPosition = functions.ReturnProperPositionV2(0 + 14, 0 + 8);
        return true;
    }
    private IEnumerator ColliderWait() {
        yield return new WaitForEndOfFrame(); // need this!

        topMap.GetComponent<CompositeCollider2D>().GenerateGeometry();
        collisionMap.GetComponent<CompositeCollider2D>().GenerateGeometry();

        yield return new WaitForEndOfFrame();
    }
    private IEnumerator GenerationWait() {
        bool beachResult = false;
        beachResult = generateMaze(numR);
        while (!beachResult) {
            yield return null;
        }
        while (!PlantTrees() || !GenerateAnimal() || !GenerateObjects()) {
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        astarPath.Scan();
        SpawnRandomGeese();
    }
    public void SpawnRandomGeese() {
        int geese = Random.Range(0, width / 2);
        int attempts = 15;
        for (int i = 0; i < geese; i++) {
            // attempt to find a spot where a tree can be planted
            for (int j = 0; j < attempts; j++) {
                int x = Random.Range(0, width);
                int y = Random.Range(0, height);
                if (functions.isValidTreePlantingSpot(terrainMap, 1, x, y, 1)) {
                    spawner.SpawnAGoose(functions.ReturnProperPositionV2(x, y), 0);
                    break;
                }
            }
        }
    }
    public bool GenerateObjects() {
        spawnObject.Spawn();
        return true;
    }
    //Generate wild animals
    public bool GenerateAnimal() {
        int animals = Random.Range(0, forest.level + 10);
        int attempts = 10;

        for (int i = 0; i < animals; i++) {
            for (int j = 0; j < attempts; j++) {
                // attempt to find a spot where a tree can be planted
                int x = Random.Range(width / 8, 7 * width / 8);
                int y = Random.Range(height / 8, 7 * height / 8);
                if (functions.isValidTreePlantingSpot(terrainMap, 1, x, y, 1)) {
                    spawnWildAnimal.generateAnimal(location: functions.ReturnProperPositionV2(x, y), new List<Type> { Type.FISH }, rarity: (int) System.Math.Floor(forest.level * 0.2));
                    break;
                }
            }
        }
        return true;
    }
    public bool PlantTrees() {
        int trees = Random.Range(0, width / 2);
        int attempts = 15;
        for (int i = 0; i < trees; i++) {
            // attempt to find a spot where a tree can be planted
            for (int j = 0; j < attempts; j++) {
                int x = Random.Range(0, width);
                int y = Random.Range(0, height);
                if (functions.isValidTreePlantingSpot(terrainMap, 0, x, y, 7)) {
                    spawnObject.generateTree(functions.ReturnProperPositionV2(x, y));
                    spawnObject.generatePrefab(functions.ReturnProperPositionV2(x + Random.Range(-4, 0), y + Random.Range(-4, 0)), "LAMP");
                    break;
                }
            }
        }
        return true;
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
