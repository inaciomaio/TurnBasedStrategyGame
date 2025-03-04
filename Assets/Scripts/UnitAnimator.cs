using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitAnimator : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointReference;
    [SerializeField] private Transform rifleTransform;
    [SerializeField] private Transform swordTransform;

    private void Awake() {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction)) {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction)) {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
        if (TryGetComponent<SwordAction>(out SwordAction swordAction)) {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
        }
    }

    void Start() {
        EquipRifle();
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

    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e) {
        EquipSword();
        animator.SetTrigger("SwordSlash");
    }

    private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e) {
        EquipRifle();
    }

    private void EquipSword() {
        swordTransform.gameObject.SetActive(true);
        rifleTransform.gameObject.SetActive(false);
    }

    private void EquipRifle() {
        swordTransform.gameObject.SetActive(false);
        rifleTransform.gameObject.SetActive(true);
    }
}
