using System;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour {

    public static event EventHandler OnAnyGrenadeExplosion;

    [SerializeField] private Transform GrenadeExplosionVFXPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;

    private Vector3 targetPosition;
    private Action onGrenadeBehaviourComplete;
    private float totalDistance;
    private Vector3 positionXZ;
    private void Update() {
        Vector3 moveDir = (targetPosition - positionXZ).normalized;

        float moveSpeed = 15f;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance / 3f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance) {

            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (Collider collider in colliderArray) {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit)) {
                    targetUnit.Damage(30);
                }

                if (collider.TryGetComponent<DestructableCrate>(out DestructableCrate destructableCrate)) {
                    destructableCrate.Damage();
                }
            }

            OnAnyGrenadeExplosion?.Invoke(this, EventArgs.Empty);
            trailRenderer.transform.parent = null;
            Instantiate(GrenadeExplosionVFXPrefab, targetPosition + Vector3.up * 1, Quaternion.identity);
            Destroy(gameObject);

            onGrenadeBehaviourComplete();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete) {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
