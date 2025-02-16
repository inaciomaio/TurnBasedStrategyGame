using System;
using UnityEditor.Search;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField] private GameObject actionCinemachineCamera;

    void Start() {
        BaseAction.OnAnyActionStart += BaseAction_OnAnyActionStart;
        BaseAction.OnAnyActionComplete += BaseAction_OnAnyActionComplete;

        DisableActionCamera();
    }

    private void EnableActionCamera() {
        actionCinemachineCamera.SetActive(true);
    }

    private void DisableActionCamera() {
        actionCinemachineCamera.SetActive(false);
    }

    private void BaseAction_OnAnyActionStart(object sender, EventArgs e) {
        switch (sender) {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;

                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;

                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDir * -1);
                actionCinemachineCamera.transform.position = actionCameraPosition;
                actionCinemachineCamera.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

                EnableActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionComplete(object sender, EventArgs e) {
        switch (sender) {
            case ShootAction shootAction:
                DisableActionCamera();
                break;
        }
    }
}
