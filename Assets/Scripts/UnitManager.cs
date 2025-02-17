using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    public static UnitManager Instance { get; private set; }

    private List<Unit> unitList;
    private List<Unit> friendlyUnitList;
    private List<Unit> enemyUnitList;

    void Awake() {
        Instance = this;
        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }

    private void Start() {
        Unit.OnAnyUnitSpawn += Unit_OnAnyUnitSpawn;
        Unit.OnAnyUnitDeath += Unit_OnAnyUnitDeath;
    }

    private void Unit_OnAnyUnitSpawn(object sender, EventArgs e) {
        Unit unit = sender as Unit;

        unitList.Add(unit);
        if (unit.IsEnemy()) {
            enemyUnitList.Add(unit);
        }
        else {
            friendlyUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDeath(object sender, EventArgs e) {
        Unit unit = sender as Unit;

        unitList.Remove(unit);
        if (unit.IsEnemy()) {
            enemyUnitList.Remove(unit);
        }
        else {
            friendlyUnitList.Remove(unit);
        }
    }

    public List<Unit> GetUnitList() {
        return unitList;
    }

    public List<Unit> GetFriendlyUnitList() {
        return friendlyUnitList;
    }

    public List<Unit> GetEnemyUnitList() {
        return enemyUnitList;
    }
}