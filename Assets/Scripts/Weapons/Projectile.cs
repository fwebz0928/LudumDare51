using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private TrailRenderer trail;
    private float _damage;
    private float _lifeTime;

    public delegate void OnDisableCallback(Projectile instance);
    public OnDisableCallback Disable;

    public void SetProjectileData(float damage, float lifeTime)
    {
        _damage = damage;
        _lifeTime = lifeTime;
    }

    public void StartLifeTimeCountdown() => Invoke(nameof(ResetProjectile), _lifeTime);

    protected virtual void ResetProjectile()
    {
        rb2d.velocity = new Vector2(0.0f, 0.0f);
        trail.Clear();
        Disable?.Invoke(this);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Enemy")) return;
        
    }
}