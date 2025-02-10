using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour {

    [SerializeField] Button endTurnButton;
    [SerializeField] TextMeshProUGUI turnText;

    private void Awake() {
        endTurnButton.onClick.AddListener(() => {
            TurnSystem.Instance.NextTurn();
        });

        UpdateText();
    }

    private void Start() {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void UpdateText() {
        turnText.text = "TURN " + TurnSystem.Instance.GetCurrentTurnNumber();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e) {
        UpdateText();
    }

}
