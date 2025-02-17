using TMPro;
using UnityEngine;

public class PathfindingGridDebugObject : GridDebugObject {
    [SerializeField] private TextMeshPro gCosttext;
    [SerializeField] private TextMeshPro hCosttext;
    [SerializeField] private TextMeshPro fCosttext;

    private PathNode pathNode;

    public override void SetGridObject(object gridObject) {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;
    }

    protected override void Update() {
        base.Update();
        gCosttext.text = pathNode.GetGCost().ToString();
        hCosttext.text = pathNode.GetHCost().ToString();
        fCosttext.text = pathNode.GetFCost().ToString();
    }
}
