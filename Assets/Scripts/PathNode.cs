using UnityEngine;

public class PathNode : MonoBehaviour {
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode previousPathNode;
    public PathNode(GridPosition gridPosition) {
        this.gridPosition = gridPosition;
    }

    public override string ToString() {
        return gridPosition.ToString();
    }

    public int GetGCost() {
        return gCost;
    }

    public int GetHCost() {
        return hCost;
    }

    public int GetFCost() {
        return fCost;
    }
}
