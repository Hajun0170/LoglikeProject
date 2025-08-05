using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;              // 무기 종류 (enum)
    public WeaponData weaponData;              // SO: 투사체 속도, 연사속도 등
    public TargetTracker targetTracker;        // 타겟 감지용

    public int level = 1;                      // 업그레이드용

    private float nextFireTime = 0f;

    void Update()
    {
        if (weaponData == null || targetTracker == null)
            return;

        if (Time.time >= nextFireTime)
        {
            Transform target = targetTracker.GetFirstTarget();
            if (target != null)
            {
                Fire(target.position);
                nextFireTime = Time.time + 1f / weaponData.fireRate;
            }
        }
    }

    void Fire(Vector2 targetPosition)
    {
        GameObject projectileGO = Instantiate(
            weaponData.projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        Projectile projectile = projectileGO.GetComponent<Projectile>();
        if (projectile != null)
        {
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            if (direction == Vector2.zero)
                direction = Vector2.right;

            projectile.Seek(direction, weaponData.projectileSpeed);
        }
    }

    public void Upgrade()
    {
        level++;
        weaponData.fireRate += 0.2f; // 예시: 연사속도 증가
        Debug.Log($"🛠 무기 강화! Lv.{level}, fireRate: {weaponData.fireRate:F2}");
    }
}
