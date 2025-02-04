using System;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour {
    [SerializeField] private Unit unit;

    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        UpdateVisual();
    }

    private void Start() {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        if (unit == UnitActionSystem.Instance.GetSelectedUnit()) {
            meshRenderer.enabled = true;
        }
        else {
            meshRenderer.enabled = false;
        }
    }
}
