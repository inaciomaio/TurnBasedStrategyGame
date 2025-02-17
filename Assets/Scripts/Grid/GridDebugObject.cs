using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour {

    [SerializeField] TextMeshPro debugText;
    private object gridObject;

    public virtual void SetGridObject(object gridObject) {
        this.gridObject = gridObject;
    }

    protected virtual void Update() {
        debugText.text = gridObject.ToString();
    }

}
