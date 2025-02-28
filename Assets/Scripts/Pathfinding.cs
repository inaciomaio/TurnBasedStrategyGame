using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour {

    public static Pathfinding Instance;

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;
    private int width;
    private int height;
    private float cellsize;
    private GridSystem<PathNode> gridSystem;

    void Awake() {
        Instance = this;
    }

    void Start() {
        //gridSystem = new GridSystem<PathNode>(width, height, cellsize, (GridSystem<PathNode> gameObject, GridPosition gridPosition) => new PathNode(gridPosition));
    }

    public void Setup(int width, int height, float cellsize) {
        this.width = width;
        this.height = height;
        this.cellsize = cellsize;

        gridSystem = new GridSystem<PathNode>(width, height, cellsize,
        (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up, raycastOffsetDistance * 2, obstaclesLayerMask)) {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }

        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength) {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++) {
            for (int z = 0; z < gridSystem.GetHeight(); z++) {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetPreviousPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0) {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode) {
                // reached final node
                pathLength = endNode.GetFCost();
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeightbourList(currentNode)) {
                if (closedList.Contains(neighbourNode)) {
                    continue;
                }

                if (!neighbourNode.GetIsWalkable()) {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost =
                    currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if (tentativeGCost < neighbourNode.GetGCost()) {
                    neighbourNode.SetPreviousPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // No Path Found
        pathLength = 0;
        return null;
    }

    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB) {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remainder = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remainder;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList) {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost()) {
                lowestFCostPathNode = pathNodeList[i];
            }
        }
        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z) {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    private List<PathNode> GetNeightbourList(PathNode currentNode) {
        List<PathNode> neightboutList = new List<PathNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();

        if (gridPosition.x - 1 >= 0) {
            //Left Node
            neightboutList.Add(GetNode(gridPosition.x - 1, gridPosition.z));

            if (gridPosition.z - 1 >= 0) {
                //Left Down Node
                neightboutList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }

            if (gridPosition.z + 1 < gridSystem.GetHeight()) {
                //Left UP Node
                neightboutList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }
        }

        if (gridPosition.x + 1 < gridSystem.GetWidth()) {
            //Right Node
            neightboutList.Add(GetNode(gridPosition.x + 1, gridPosition.z));
            if (gridPosition.z - 1 >= 0) {
                //Right Down Node
                neightboutList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight()) {
                //Right UP Node
                neightboutList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
        }

        if (gridPosition.z - 1 >= 0) {
            //Down
            neightboutList.Add(GetNode(gridPosition.x, gridPosition.z - 1));
        }

        if (gridPosition.z + 1 < gridSystem.GetHeight()) {
            //Up
            neightboutList.Add(GetNode(gridPosition.x, gridPosition.z + 1));
        }
        return neightboutList;
    }

    private List<GridPosition> CalculatePath(PathNode endNode) {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.GetPreviousPathNode() != null) {
            pathNodeList.Add(currentNode.GetPreviousPathNode());
            currentNode = currentNode.GetPreviousPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList) {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition) {
        return gridSystem.GetGridObject(gridPosition).GetIsWalkable();
    }

    public void SetIsWalkableGridPosition(GridPosition gridPosition, bool isWalkable) {
        gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition) {
        return FindPath(startGridPosition, endGridPosition, out int pathLength) != null;
    }

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition) {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
    }
}
