using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthSystem : MonoBehaviour {
    public event EventHandler OnDeath;
    public event EventHandler OnDamaged;
    [SerializeField] private float health = 100;
    private float healthMax;

    void Awake() {
        healthMax = health;
    }

    public void Damage(float damageAmount) {
        health -= damageAmount;

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health < 0) {
            health = 0;
        }

        if (health == 0) {
            Die();
        }

    }

    private void Die() {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized() {
        return health / healthMax;
    }
}
