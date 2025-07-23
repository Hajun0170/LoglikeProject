using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;             // 무기 정보 SO
    public TargetTracker targetTracker;       // 추적 영역 연결

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
        // 투사체 생성
        GameObject projectileGO = Instantiate(
            weaponData.projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        // 투사체 스크립트 가져오기
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        if (projectile == null)
        {
            Debug.LogError("Projectile.cs가 프리팹에 없습니다!");
            return;
        }

        // 방향 계산
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        if (direction == Vector2.zero)
        {
            direction = Vector2.right; // 방향이 0일 경우 대비
            Debug.LogWarning("방향 벡터가 0입니다. 기본 방향으로 발사.");
        }

        // 투사체에 방향 전달
        projectile.Seek(direction, weaponData.projectileSpeed);
    }
}
