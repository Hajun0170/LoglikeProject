using UnityEngine;

public class FireWeapon : MonoBehaviour, IWeapon
{

    public WeaponType weaponType = WeaponType.Ranged;
    public GameObject projectilePrefab;
    public float fireRate = 1f;
    public float projectileSpeed = 10f;

    private float nextFireTime = 0f;
    private TargetTracker targetTracker;

    public int level = 1;

    void Start()
    {
        targetTracker = FindObjectOfType<TargetTracker>();
    }

    void Update()
    {
        if (targetTracker == null) return;

        if (Time.time >= nextFireTime)
        {
            Transform target = targetTracker.GetFirstTarget();
            if (target != null)
            {
                Fire(target.position);
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    void Fire(Vector2 targetPosition)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("❌ projectilePrefab이 비어있습니다!");
            return;
        }

        Vector2 baseDir = (targetPosition - (Vector2)transform.position).normalized;
        if (baseDir == Vector2.zero) baseDir = Vector2.right;

        int projectileCount = Mathf.Min(1 + (level - 1), 5); // 최대 5발까지
        float spreadAngle = 10f; // 각도 간격

        for (int i = 0; i < projectileCount; i++)
        {
            float angleOffset = (i - (projectileCount - 1) / 2f) * spreadAngle;
            Vector2 rotatedDir = Quaternion.Euler(0, 0, angleOffset) * baseDir;

            GameObject go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            FireProjectile proj = go.GetComponent<FireProjectile>();
            if (proj != null)
            {
                proj.Seek(rotatedDir, projectileSpeed);
            }
        }
    }

    public void UpgradeWeapon()
    {
        level++;
        Debug.Log($"Weapon 업그레이드됨: Lv.{level}");
    }

    // ✅ 누락된 함수 추가
    public WeaponType GetWeaponType()
    {
         return WeaponType.Ranged;
    }
}

