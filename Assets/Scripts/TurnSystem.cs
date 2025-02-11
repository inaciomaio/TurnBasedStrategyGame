using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour {

    public static TurnSystem Instance { get; private set; }

    public EventHandler OnTurnChanged;

    private int turnNumber = 1;
    private bool isPlayerTurn = true;

    private void Awake() {
        Instance = this;
    }

    public void NextTurn() {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetCurrentTurnNumber() {
        return turnNumber;
    }

    public bool IsPlayerTurn() {
        return isPlayerTurn;
    }
}
