using UnityEngine;

public class Projectile : Poolable
{
    public float speed = 10f;
    public int damage = 10;
    public float lifeTime = 5f;

    private Rigidbody2D rb;
    private float timer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            gameObject.SetActive(false); // 풀로 반환
        }
    }

    public void Seek(Vector2 direction, float overrideSpeed)
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.velocity = direction.normalized * overrideSpeed;
        timer = 0f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            gameObject.SetActive(false); // 풀로 반환
        }
    }

    public override void OnReuse()
    {
        timer = 0f;
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.velocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
    }
}
