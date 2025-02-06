using UnityEngine;

public class Unit : MonoBehaviour {
    private GridPosition currentGridPosition;
    private MoveAction moveAction;

    private void Awake() {
        moveAction = GetComponent<MoveAction>();
    }

    private void Start() {
        currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(currentGridPosition, this);
    }

    private void Update() {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != currentGridPosition) {
            LevelGrid.Instance.UnitMovedGridPosition(this, currentGridPosition, newGridPosition);
            currentGridPosition = newGridPosition;
        }
    }

    public MoveAction GetMoveAction() {
        return moveAction;
    }

    public GridPosition GetGridPosition() {
        return currentGridPosition;
    }
}
