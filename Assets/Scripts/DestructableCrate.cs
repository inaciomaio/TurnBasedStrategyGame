using System;
using UnityEngine;

public class DestructableCrate : MonoBehaviour {

    public static EventHandler OnAnyDestroy;

    private GridPosition gridPosition;

    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition() {
        return gridPosition;
    }

    public void Damage() {
        Destroy(gameObject);

        OnAnyDestroy?.Invoke(this, EventArgs.Empty);
    }
}
