using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Maxmin {
    public Vector2 max = new Vector2(-100000, -100000);
    public Vector2 min = new Vector2(100000, 100000);
    // update max and min values
    public void updateMaxMin(Vector2 position) {

        if (position.x < min.x) { min.x = position.x; }
        if (position.x > max.x) { max.x = position.x; }
        if (position.y < min.y) { min.y = position.y; }
        if (position.y > max.y) { max.y = position.y; }
    }
    public bool withinMaxMin(Vector2 position, int error = 0) {
        if (position.x + error > min.x && position.x - error < max.x) {
        } else {
            return false;
        }
        if (position.y + error > min.y && position.y - error < max.y) {
        } else {
            return false;
        }
        return true;
    }
}