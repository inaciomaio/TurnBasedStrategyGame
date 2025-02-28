using System;
using UnityEngine;

public class PathFindingUpdater : MonoBehaviour {

    private void Start() {
        DestructableCrate.OnAnyDestroy += DestructableCrate_OnAnyDestroyed;
    }

    private void DestructableCrate_OnAnyDestroyed(object sender, EventArgs e) {
        DestructableCrate destructableCrate = sender as DestructableCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructableCrate.GetGridPosition(), true);
    }
}
