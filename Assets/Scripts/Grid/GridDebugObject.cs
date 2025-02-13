using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour {

    [SerializeField] TextMeshPro debugText;
    private GridObject gridObject;

    public void SetGridObject(GridObject gridObject) {
        this.gridObject = gridObject;
    }

    private void Update() {
        debugText.text = gridObject.ToString();
    }

}
