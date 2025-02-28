using System;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour {
    void Start() {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExplosion += GrenadeProjectile_OnAnyGrenadeExplosion;
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e) {
        CameraShake.Instance.Shake();
    }

    private void GrenadeProjectile_OnAnyGrenadeExplosion(object sender, EventArgs e) {
        CameraShake.Instance.Shake(5f);
    }
}
