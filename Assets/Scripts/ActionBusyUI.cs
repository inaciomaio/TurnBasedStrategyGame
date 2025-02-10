using System;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour {

    private void Start() {
        Hide();
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy) {
        if (isBusy) {
            Show();
        }
        else {
            Hide();
        }
    }

}
