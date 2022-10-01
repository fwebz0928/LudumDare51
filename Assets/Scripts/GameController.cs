using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] private WeaponBase[] weaponsDB;
    [SerializeField] private Enemy enemyGo;
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
    }
    private Enemy CreatePoolObject()
    {
        var enemy = Instantiate(enemyGo, transform.position, quaternion.identity);
        this.gameObject.SetActive(false);
        return null;
    }
    private void OnTakeFromPool(Enemy obj)
    {
        throw new NotImplementedException();
    }
    private void OnReturnToPool(Enemy obj)
    {
        throw new NotImplementedException();
    }
    private void OnDestroyObject(Enemy obj)
    {
        throw new NotImplementedException();
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
}