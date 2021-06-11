using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationSharedFunctions {
    int width;
    int height;
    public void Initiate(int width, int height) {
        this.width = width;
        this.height = height;
    }
    public Vector3Int ReturnProperPosition(int x, int y, int z) {
        return new Vector3Int(x - (width / 2), y - (height / 2), z);
    }
    public Vector2 ReturnProperPositionV2(int x, int y) {
        return new Vector2(x - (width / 2), y - (height / 2));
    }
    public int[,] drawMaze(int[,] map, SpawnObject spawnObject, Forest forest = null, bool spawnStar = false) {
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
                if (forest && curStarCount < forest.starCount && spawnStar) {
                    spawnObject.generatePrefab(ReturnProperPositionV2((int) (randomExit.x * 2) + 1, (int) (randomExit.y * 2) + 1), "STAR");
                    curStarCount++;
                }
            }
        }
        if (forest && spawnStar) {
            for (int i = 0; i < forest.starCount - curStarCount; i++) {
                bool generated = false;
                while (generated == false) {
                    int x = Random.Range(1, width - 1);
                    int y = Random.Range(1, height - 1);
                    if (isValidTreePlantingSpot(map, 1, x, y, 1)) {
                        generated = true;
                        spawnObject.generatePrefab(ReturnProperPositionV2((int) (x * 2) + 1, (int) (y * 2) + 1), "STAR");
                    }
                }
            }
        }
        return map;
    }
    public bool isValidTreePlantingSpot(int[,] map, int value, int x, int y, int size = 1) {
        return AreaIs1(value, map, x, y, size);
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
    public int[,] setZeroes(int[,] map) {
        for (int x = 0; x < map.GetLength(0); x++) {
            for (int y = 0; y < map.GetLength(1); y++) {
                map[x, y] = 0;
            }
        }
        return map;
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
    public int[,] drawRectangle(int[,] map, int x = -1, int y = -1, bool beginning = false, bool center = false, int value = 1) {
        int rectWidth = x == -1 ? Random.Range(2, width / 2) : x;
        int rectHeight = y == -1 ? Random.Range(2, height / 2) : y;
        // if draw empty area at start
        Vector2 pickPoint = beginning ? new Vector2(1, 1) :
        center ? new Vector2((width / 2) - (rectWidth / 2), (height / 2) - (rectHeight / 2)) :
        new Vector2(Random.Range(1, width), Random.Range(1, height));
        for (int i = 0; i < rectWidth; i++) {
            for (int j = 0; j < rectHeight; j++) {
                if (pickPoint.x + i >= width - 1 || pickPoint.y + j >= height - 1) {
                    break;
                }
                map[(int) pickPoint.x + i, (int) pickPoint.y + j] = value;
            }
        }
        return map;
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
}
