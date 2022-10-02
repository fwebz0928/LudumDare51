using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health = 100.0f;
    [SerializeField] protected float moveSpeed = 4.0f;
    [SerializeField] protected Rigidbody2D rb2d;
    [SerializeField] protected float maxDistance;

    private bool _bCanDamage;

    public delegate void Disable(Enemy instance);
    public Disable DisableEnemyDel;

    private Vector3 _dir;
    private float _angle;
    protected Vector2 MoveDir;

    private void Awake()
    {
        moveSpeed = Random.Range(3.2f, 4.0f);
    }

    private void Update()
    {
        // transform.position = Vector3.MoveTowards(transform.position, PlayerController.Instance.gameObject.transform.position, moveSpeed * Time.deltaTime);
        _dir = (PlayerController.Instance.transform.position - transform.position).normalized;
        //angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //rb2d.rotation = angle;
        MoveDir = _dir;
    }
    private void FixedUpdate() => Move();

    protected virtual void Move()
    {
        var distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        rb2d.velocity = new Vector2(MoveDir.x, MoveDir.y) * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        _bCanDamage = true;
        StartCoroutine(DamageOverTime());
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        _bCanDamage = false;
    }

    private IEnumerator DamageOverTime()
    {
        while (_bCanDamage)
        {
            yield return new WaitForSeconds(.2f);
            PlayerController.Instance.TakeDamage(2.0f);
        }
    }
    public void TakeDamage(float damage)
    {
        health = Mathf.Clamp(health -= damage, 0.0f, 1000.0f);
        if (health <= 0)
        {
            DisableEnemyDel?.Invoke(this);
            health = 100.0f;
        }
    }
}