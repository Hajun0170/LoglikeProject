using UnityEngine;

public class BladeDamage : MonoBehaviour
{
    public int iDamage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // 적에게 데미지 주는 로직
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(iDamage);
        }
    }
}
