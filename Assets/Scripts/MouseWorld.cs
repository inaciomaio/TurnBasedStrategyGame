using UnityEngine;

public class MouseWorld : MonoBehaviour {

    private static MouseWorld instance;
    [SerializeField] private Transform debugSphere;

    [SerializeField] private LayerMask mousePlaneLayerMask;

    private void Awake() {
        instance = this;
    }

    //private void Update() {
    //    debugSphere.transform.position = GetPosition();
    //    Debug.Log(GetPosition());
    //}

    public static Vector3 GetPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask);
        return raycastHit.point;
    }
}
