using System;
using UnityEngine;

public class DestructableCrate : MonoBehaviour {

    public static EventHandler OnAnyDestroy;

    [SerializeField] private Transform crateDestroyedPrefab;
    private GridPosition gridPosition;

    private void Start() {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition() {
        return gridPosition;
    }

    public void Damage() {
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);

        ApplyExplosionToChildren(crateDestroyedTransform, 150f, transform.position, 10f);

        Destroy(gameObject);

        OnAnyDestroy?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRadius) {
        foreach (Transform child in root) {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody)) {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }

            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRadius);
        }
    }
}
