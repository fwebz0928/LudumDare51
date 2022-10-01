using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    //Movement
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxhealth;
    public WeaponBase currentWeapon;
    [SerializeField] private Transform weaponPos;
    [SerializeField] private Transform weaponPivot;

    private float _currentHealth;
    private Vector2 _movement;
    private float _lastFired;

    public delegate void OnPlayerHealthUpdated(float amount);
    public OnPlayerHealthUpdated UpdateHealthBarDel;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UpdateHealthBarDel?.Invoke(_currentHealth / maxhealth);
    }

    private void Update()
    {
        //Movement
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        //Weapon Rotation
        UpdateWeaponPivot();

        //Firing Weapon
        if (currentWeapon == null) return;
        if (Input.GetButton("Fire1"))
        {
            if (!(Time.time - _lastFired > 1 / currentWeapon.fireRate)) return;
            _lastFired = Time.time;
            currentWeapon.Fire();
        }
    }

    private void FixedUpdate()
    {
        rb2D.velocity = new Vector2(_movement.x * movementSpeed,
            _movement.y * movementSpeed);
    }

    public void TakeDamage(float damageAmount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damageAmount, 0.0f, maxhealth);
        UpdateHealthBarDel?.Invoke(_currentHealth / maxhealth);
        if (_currentHealth <= 0.0f)
        {
            //Do Death things
        }
    }

    public void UpdateWeapon(WeaponBase newWeapon)
    {
        if (currentWeapon != null)
            Destroy(currentWeapon.gameObject);
        currentWeapon = Instantiate(newWeapon, weaponPos.position, weaponPivot.transform.rotation);
        currentWeapon.transform.parent = weaponPos.transform;
    }

    private void UpdateWeaponPivot()
    {
        //Update Pivot Rotation
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - weaponPivot.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 0.0f, Vector3.forward);
        weaponPivot.transform.rotation = rotation;

        if (weaponPivot.transform.localEulerAngles.z > 270.0f || weaponPivot.transform.localEulerAngles.z < 90.0f) //If Angle is positive (RightSide of Player)
        {
            if (currentWeapon != null)
                currentWeapon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if (weaponPivot.transform.localEulerAngles.z < 270.0f || weaponPivot.transform.localEulerAngles.z > 90.0f) // if angle is negative (Left Side of Player)
            if (currentWeapon)
                currentWeapon.transform.localScale = new Vector3(1.0f, -1.0f, 1.0f);
    }
}