using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] private bool bUsePooling = true;
    [SerializeField] private Projectile projectile;
    [SerializeField] private int projectileAmount;
    [SerializeField] private Transform fireLoc;
    [SerializeField] private float lifeTime;
    [SerializeField] private float damage;
    [SerializeField] private float projectileSpeed;
    public float fireRate = 1.0f;

    private ObjectPool<Projectile> _projectilePool;
    private void Start()
    {
        if (bUsePooling)
            _projectilePool = new ObjectPool<Projectile>
            (
                CreatePoolObject,
                OnTakeFromPool,
                OnReturnToPool,
                OnDestroyObject,
                true,
                10,
                10000
            );
    }
    private Projectile CreatePoolObject()
    {
        var instance = Instantiate(projectile, Vector3.zero, Quaternion.identity);
        instance.Disable += ReturnObjectToPool;
        instance.gameObject.SetActive(false);
        instance.SetProjectileData(damage, lifeTime);
        //instance.transform.SetParent(transform, false);
        return instance;
    }
    private void ReturnObjectToPool(Projectile instance) => _projectilePool.Release(instance);
    private void OnTakeFromPool(Projectile obj)
    {
        obj.gameObject.SetActive(true);
        FireProjectile(obj);
    }
    private void OnReturnToPool(Projectile obj) => obj.gameObject.SetActive(false);
    private void OnDestroyObject(Projectile obj)
    {
    }

    public void Fire()
    {
        for (var i = 0; i < projectileAmount; i++)
            _projectilePool.Get();
    }
    protected virtual void FireProjectile(Projectile obj)
    {
        obj.transform.position = fireLoc.position;
        obj.GetComponent<Rigidbody2D>().AddForce(fireLoc.right * projectileSpeed, ForceMode2D.Impulse);
        obj.StartLifeTimeCountdown();
    }
}