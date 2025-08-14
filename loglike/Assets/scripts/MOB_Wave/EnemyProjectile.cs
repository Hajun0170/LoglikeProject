using UnityEngine;

public class EnemyProjectile : Poolable
{
    public float speed = 6f;
    public float lifetime = 3f;

    private Vector2 direction;
    private float lifeTimer;

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifetime)
        {
            gameObject.SetActive(false);
        }
    }

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public override void OnReuse()
    {
        lifeTimer = 0f;
        direction = Vector2.zero; // 필요에 따라 초기화
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats ps = other.GetComponent<PlayerStats>();
            if (ps != null)
            {
                ps.TakeDamage(1);
            }

            gameObject.SetActive(false);
        }
    }
}
