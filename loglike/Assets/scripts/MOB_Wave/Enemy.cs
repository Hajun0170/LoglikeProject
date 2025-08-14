using UnityEngine;

 public class Enemy : Poolable
{
    [Header("Stats")]
    public float moveSpeed = 2f;
    public int maxHP = 10;
    protected int currentHP;

    [Header("References")]
    protected Transform player;
    public GameObject expGemPrefab;

    public override void OnReuse()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentHP = maxHP;
    }

    protected virtual void Update()
    {
        MoveTowardPlayer();
    }

    protected virtual void MoveTowardPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    public virtual void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (expGemPrefab != null)
        {
            PoolManager.Instance.Spawn("ExpGem", transform.position, Quaternion.identity);
        }

        gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(1);
            }
        }
    }
}
