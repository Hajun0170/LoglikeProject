using UnityEngine;

public class RangedEnemy : Enemy
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 6f;
    public float attackCooldown = 2f;

    private float nextAttackTime;

    protected override void Update()
    {
        base.Update(); // 기본 이동

        if (Time.time >= nextAttackTime)
        {
            FireProjectile();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

 void FireProjectile()
{
    if (player == null) return;

    Vector2 dir = (player.position - transform.position).normalized;

    GameObject proj = PoolManager.Instance.Spawn("EnemyProjectile", transform.position, Quaternion.identity);
    proj.GetComponent<EnemyProjectile>().Init(dir);
}
}
