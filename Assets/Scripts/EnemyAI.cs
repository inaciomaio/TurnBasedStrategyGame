using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    private enum State {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State state;
    private float timer;

    void Awake() {
        state = State.WaitingForEnemyTurn;
    }

    private void Start() {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update() {
        if (TurnSystem.Instance.IsPlayerTurn()) {
            return;
        }

        switch (state) {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= -0f) {
                    state = State.Busy;
                    if (TryTakeEnemyAIAction(SetStateTakingTurn)) {
                        state = State.Busy;
                    }
                    else {
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }


    }

    private void SetStateTakingTurn() {
        timer = .5f;
        state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) {
        if (!TurnSystem.Instance.IsPlayerTurn()) {
            state = State.TakingTurn;
            timer = 2f;
        }

    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete) {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList()) {
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete)) {
                return true;
            }
        }
        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete) {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;
        foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray()) {
            if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction)) {
                // enemy cannot afford this action
                continue;
            }

            if (bestEnemyAIAction == null) {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else {
                EnemyAIAction testEnemyAiAction = baseAction.GetBestEnemyAIAction();
                if (testEnemyAiAction != null && testEnemyAiAction.actionValue > bestEnemyAIAction.actionValue) {
                    bestEnemyAIAction = testEnemyAiAction;
                    bestBaseAction = baseAction;
                }
            }

        }

        if (bestEnemyAIAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction)) {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else {
            return false;
        }
    }
}
