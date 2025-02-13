using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    [SerializeField] private bool invert;
    private void LateUpdate() {

        if (invert) {
            Vector3 dirToCamera = (Camera.main.transform.position - transform.position).normalized;
            transform.LookAt(transform.position + dirToCamera * -1);
        }
        else {
            transform.LookAt(Camera.main.transform);
        }

    }
}
