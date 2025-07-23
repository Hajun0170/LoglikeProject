using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int maxHP = 10;
    private int currentHP;

    private Transform player;

    public GameObject expGemPrefab; // 드랍할 경험치 프리팹

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHP = maxHP;
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
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
            Instantiate(expGemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // 추후에 오브젝트 풀링 적용 가능
    }
}
