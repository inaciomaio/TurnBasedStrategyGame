using UnityEngine;

public class PathNode {
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private PathNode previousPathNode;
    private bool isWalkable = true;
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

    public void SetGCost(int gCost) {
        this.gCost = gCost;
    }

    public void SetHCost(int hCost) {
        this.hCost = hCost;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    public void ResetPreviousPathNode() {
        previousPathNode = null;
    }

    public void SetPreviousPathNode(PathNode pathNode) {
        previousPathNode = pathNode;
    }

    public PathNode GetPreviousPathNode() {
        return previousPathNode;
    }

    public GridPosition GetGridPosition() {
        return gridPosition;
    }

    public bool GetIsWalkable() {
        return isWalkable;
    }

    public void SetIsWalkable(bool isWalkable) {
        this.isWalkable = isWalkable;
    }

}
