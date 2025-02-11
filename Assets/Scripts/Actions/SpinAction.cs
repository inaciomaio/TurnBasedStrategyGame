using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class SpinAction : BaseAction {
    private float totalSpinAmount;

    private void Update() {

        if (!isActive) return;

        float spinAddAmount = 360f * Time.deltaTime;
        totalSpinAmount += 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        if (totalSpinAmount > 360f) {
            totalSpinAmount = 0f;
            ActionComplete();
        }
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        ActionStart(onActionComplete);
    }

    public override string GetActionName() {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList() {

        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition> {
            unitGridPosition
        };
    }
}
