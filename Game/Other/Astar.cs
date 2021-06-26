using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Pathfinding;

public class Astar : CustomMonoBehaviour {
    [Tooltip("Astar graph scans are batched up and executed at most once every `scanDelay` seconds.")]
    public float scanDelay;

    private bool graphScanInProgress;


    /// RescanAstarGraph functions should be invoked when a change happens
    /// in the "obstacle" layer that requires the A* graph to be regenerated.

    /// Rescan the entire graph (costly, avoid doing often)
    /// `force = true` means we should scan immediately,
    /// whereas the default behavior is to schedule a scan to run in the future,
    /// which adds some debouncing behavior if multiple calls to RescanAstarGraph
    /// happen in close succession.
    public void RescanAstarGraph(bool force = false) {
        if (force) {
            AstarPath.active.Scan();
        } else if (!graphScanInProgress) {
            StartCoroutine(DelayedScan(scanDelay));
        }
    }

    private IEnumerator DelayedScan(float delayTime) {
        graphScanInProgress = true;
        yield return new WaitForSeconds(delayTime);
        AstarPath.active.Scan();
        graphScanInProgress = false;
    }

    /// Used to update the graph around some newly created object
    /// e.g.: RescanAStarGraph(gameObject.GetComponent<Collider2D>().bounds)
    public void RescanAstarGraph(Bounds boundsToRescan) {
        // TODO: this doesn't really work!!! sometimes it does, sometimes it doesn't
        // seems to work for placing down buildings, but not prefabs via prefab controller (eg trees)
        // and works for tiles
        // edit: think it's when the bounds is too small (or maybe if the collider is too small?)
        // when i make the collider of a tree bigger, it works, but it doesnt work otherwise
        var guo = new GraphUpdateObject(boundsToRescan);
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);
    }
}
