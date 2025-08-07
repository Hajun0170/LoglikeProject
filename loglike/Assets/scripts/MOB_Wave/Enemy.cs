using UnityEngine;

public class Enemy : Poolable
{
    [Header("Stats")]
    public float moveSpeed = 2f;
    public int maxHP = 10;
    private int currentHP;

    [Header("References")]
    private Transform player;
    public GameObject expGemPrefab;

    public override void OnReuse()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentHP = maxHP;
    }

    void Update()
    {
        MoveTowardPlayer();
    }

    void MoveTowardPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (expGemPrefab != null)
        {
            PoolManager.Instance.Spawn("ExpGem", transform.position, Quaternion.identity);
        }

        gameObject.SetActive(false); // ✅ Destroy 대신 풀 반환
    }

    private void OnTriggerEnter2D(Collider2D other)
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
