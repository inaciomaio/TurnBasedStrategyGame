using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitAnimator : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointReference;

    private void Awake() {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction)) {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction)) {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e) {
        animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e) {
        animator.SetBool("IsWalking", false);
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e) {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointReference.position, quaternion.identity);
        bulletProjectileTransform.TryGetComponent<BulletProjectile>(out BulletProjectile bulletProjectile);

        Vector3 targetUnitShootPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootPosition.y = shootPointReference.position.y;
        bulletProjectile.Setup(targetUnitShootPosition);
    }
}
