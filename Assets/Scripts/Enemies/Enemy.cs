using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100.0f;
    [SerializeField] private float moveSpeed = 4.0f;

    public delegate void Disable(Enemy instance);
    public Disable DisableEnemyDel;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, PlayerController.Instance.gameObject.transform.position, moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        health = Mathf.Clamp(health -= damage, 0.0f, 1000.0f);
        if (health <= 0)
        {
            DisableEnemyDel?.Invoke(this);
            health = 100.0f;
            //Kill enemy  / reset with object pooling
        }
    }
}