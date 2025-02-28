using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public static CameraShake Instance { get; private set; }
    private CinemachineImpulseSource cinemachineImpulseSource;

    void Awake() {
        Instance = this;
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity = 1f) {
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
