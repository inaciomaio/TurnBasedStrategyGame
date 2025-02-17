using UnityEngine;

public class Pathfinding : MonoBehaviour {
    [SerializeField] private Transform gridDebugObjectPrefab;
    private int width;
    private int height;
    private int cellsize;
    private GridSystem<PathNode> gridSystem;

    void Awake() {
        gridSystem = new GridSystem<PathNode>(10, 10, 2f, (GridSystem<PathNode> gameObject, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }
}
