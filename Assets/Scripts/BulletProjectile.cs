using Unity.Mathematics;
using UnityEngine;

public class BulletProjectile : MonoBehaviour {
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletVFX;
    private Vector3 targetPosition;
    public void Setup(Vector3 targetPosition) {
        this.targetPosition = targetPosition;
    }

    void Update() {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);
        float movementSpeed = 200f;
        transform.position += moveDir * movementSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving) {
            transform.position = targetPosition;
            Instantiate(bulletVFX, transform.position, Quaternion.identity);
            trailRenderer.transform.SetParent(null);
            Destroy(gameObject);
        }
    }
}
