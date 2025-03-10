using System.Collections.Generic;
using UnityEngine;

public class GridObject {

    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;
    private Door door;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition) {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Unit>();
    }

    public override string ToString() {
        string unitStirng = "";
        foreach (Unit unit in unitList) {
            unitStirng += $"{unit} \n ";
        }

        return $"{gridPosition.ToString()} \n {unitStirng}";
    }

    public void AddUnit(Unit unit) {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit) {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList() {
        return unitList;
    }

    public bool HasAnyUnit() {
        return unitList.Count > 0;
    }

    public Unit GetUnit() {
        if (HasAnyUnit()) {
            return unitList[0];
        }
        else {
            return null;
        }
    }

    public Door GetDoor() {
        return door;
    }

    public void SetDoor(Door door) {
        this.door = door;
    }
}
