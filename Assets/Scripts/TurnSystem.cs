using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour {

    public static TurnSystem Instance { get; private set; }

    public EventHandler OnTurnChanged;

    private int turnNumber = 1;

    private void Awake() {
        Instance = this;
    }

    public void NextTurn() {
        turnNumber++;
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetCurrentTurnNumber() {
        return turnNumber;
    }
}
