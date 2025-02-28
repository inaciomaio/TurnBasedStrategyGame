using UnityEngine;

public class UnitRagdoll : MonoBehaviour {
    [SerializeField] private Transform ragdollRootBone;

    public void Setup(Transform originalRootBone) {
        MatchAllChildTransforms(originalRootBone, ragdollRootBone);

        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        ApplyExplosionToRagdoll(ragdollRootBone, 300f, transform.position + randomDir, 10f);
    }

    private void MatchAllChildTransforms(Transform root, Transform target) {
        foreach (Transform child in root) {
            Transform targetChild = target.Find(child.name);
            if (targetChild != null) {
                targetChild.position = child.position;
                targetChild.rotation = child.rotation;

                MatchAllChildTransforms(child, targetChild);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRadius) {
        foreach (Transform child in root) {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody)) {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRadius);
        }
    }
}
