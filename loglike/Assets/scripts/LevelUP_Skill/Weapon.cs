using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType weaponType = WeaponType.Ranged;
    public GameObject projectilePrefab;  // 프리팹 연결
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

        GameObject go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Projectile proj = go.GetComponent<Projectile>();
        if (proj != null)
        {
            Vector2 dir = (targetPosition - (Vector2)transform.position).normalized;
            if (dir == Vector2.zero) dir = Vector2.right;

            proj.Seek(dir, projectileSpeed);
        }
    }

    public void Upgrade()
    {
        level++;
        fireRate += 0.2f;
        Debug.Log($"📈 원거리 무기 강화! Lv.{level}, fireRate: {fireRate:F2}");
    }
}
