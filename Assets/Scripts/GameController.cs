using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] private WeaponBase[] weaponsDB;
    [SerializeField] private List<EnemySpawnData> enemySpawnData = new List<EnemySpawnData>();
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private float spawnRate = 10.0f;
    [SerializeField] private int spawnAmount = 4;
    [HideInInspector] public float timerAmount = 10.0f;

    private WeaponBase _lastWeapon;
    private ObjectPool<Enemy> _enemyPool;

    public delegate void OnUpdateCountdownTimer(float time);
    public OnUpdateCountdownTimer UpdateCountdownTimerDel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Set Weapon At Start
        ChooseNewWeapon();
        StartCoroutine(ChangeWeapon());

        
        //Enemy Spawning
        _enemyPool = new ObjectPool<Enemy>
        (
            CreatePoolObject,
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyObject,
            true,
            10,
            1000000
        );

        StartCoroutine(SpawnEnemy());
    }
    private Enemy CreatePoolObject()
    {
        var enemy = Instantiate(enemies[Random.Range(0, enemies.Count)], transform.position, quaternion.identity);
        enemy.gameObject.SetActive(false);
        return enemy;
    }
    private void OnTakeFromPool(Enemy obj)
    {
        obj.gameObject.SetActive(true);
        obj.transform.position = GetRandomPointNearPlayer(14.0f, 2.0f);
    }
    private void ReturnObjectToPool(Enemy obj) => _enemyPool.Release(obj);
    private void OnReturnToPool(Enemy obj) => obj.gameObject.SetActive(false);
    private void OnDestroyObject(Enemy obj)
    {
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            for (int i = 0; i < spawnAmount; i++)
                _enemyPool.Get();
        }
    }

    private void Update()
    {
        if (timerAmount > 0)
            timerAmount -= Time.deltaTime;
        else
            timerAmount = 10.0f;

        UpdateCountdownTimerDel?.Invoke(timerAmount);
    }

    private void ChooseNewWeapon()
    {
        var newWeapon = weaponsDB[Random.Range(0, weaponsDB.Length)];
        while (PlayerController.Instance.currentWeapon == newWeapon)
            newWeapon = weaponsDB[Random.Range(0, weaponsDB.Length)];

        print($"new Weapon {newWeapon.name}");
        PlayerController.Instance.UpdateWeapon(newWeapon);
    }

    private IEnumerator ChangeWeapon()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f);
            ChooseNewWeapon();
        }
    }


    public Vector3 GetRandomPointNearPlayer(float range,float minDistance)
    {
        var randomPos = (Vector3)Random.insideUnitCircle * range;
        var distance = Vector3.Distance(randomPos, PlayerController.Instance.gameObject.transform.position);
        while (distance < minDistance)
        {
            randomPos = (Vector3)Random.insideUnitCircle * range;
            distance = Vector3.Distance(randomPos, PlayerController.Instance.gameObject.transform.position);
        }
        return randomPos += PlayerController.Instance.gameObject.transform.position;
    }
    
    
}

[Serializable]
class EnemySpawnData
{
    public Enemy enemy;
    public int spawnMinute;
}