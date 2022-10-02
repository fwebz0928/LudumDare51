using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MediumEnemy : Enemy
{
    [SerializeField] private EnemyProjectile enemyProjectile;

    private void Start()
    {
    }

    protected override void Move()
    {
        var distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);
        if (distance >= maxDistance)
            rb2d.velocity = new Vector2(MoveDir.x, MoveDir.y) * moveSpeed;
        else
        {
            Vector3 randPoint = (Vector3)Random.insideUnitCircle * 2.0f;
            transform.position = randPoint;
        }
    }

    private IEnumerator FireAtPlayer()
    {
        while (true)
        {
            var rand = Random.Range(0, 2.0f);
            yield return new WaitForSeconds(rand);

            var moveDir = (PlayerController.Instance.transform.position - transform.position).normalized * 10.0f;
            var firedProj = Instantiate(enemyProjectile, transform.position, quaternion.identity);
            firedProj.SetProjectileData(25.0f, 2, moveDir);
        }
    }
}