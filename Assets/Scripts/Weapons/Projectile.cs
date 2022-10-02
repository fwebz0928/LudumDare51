using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb2d;
    [SerializeField] protected TrailRenderer trail;
    protected float _damage;
    protected float _lifeTime;

    public delegate void OnDisableCallback(Projectile instance);
    public OnDisableCallback Disable;

    public void SetProjectileData(float damage, float lifeTime)
    {
        _damage = damage;
        _lifeTime = lifeTime;
    }
    public void SetProjectileData(float damage, float lifeTime, Vector2 fireDir)
    {
        _damage = damage;
        _lifeTime = lifeTime;
        rb2d.velocity = fireDir;
    }

    public void StartLifeTimeCountdown() => Invoke(nameof(ResetProjectile), _lifeTime);

    protected virtual void ResetProjectile()
    {
        rb2d.velocity = new Vector2(0.0f, 0.0f);
        trail.Clear();
        Disable?.Invoke(this);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Enemy")) return;
        col.gameObject.GetComponent<Enemy>().TakeDamage(_damage);
        ResetProjectile();
    }
}