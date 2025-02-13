using System;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour {
    [SerializeField] private Transform ragdollPrefab;
    [SerializeField] private Transform originalRootBone;
    private HealthSystem healthSystem;

    void Awake() {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e) {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone);
    }
}
