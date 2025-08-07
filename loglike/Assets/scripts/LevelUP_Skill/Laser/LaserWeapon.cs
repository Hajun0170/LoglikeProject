using UnityEngine;

public class LaserWeapon : MonoBehaviour, IWeapon
{
    public GameObject laserPrefab;
    public float fireRate = 1f;
    public float laserDuration = 0.5f;
    public float fadeTime = 0.2f;

    public float laserWidth = 0.2f;  // 레이저 굵기
    public int damage = 10;

    private float nextFireTime = 0f;
    private TargetTracker targetTracker;

   public WeaponType GetWeaponType()
{
    return WeaponType.Laser;
}
    void Start()
    {
        targetTracker = GetComponentInParent<TargetTracker>();
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FireLaser();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void FireLaser()
    {
        Transform target = targetTracker.GetFirstTarget();
        if (target == null) return;

        Vector2 dir = (target.position - transform.position).normalized;
        Quaternion rot = Quaternion.FromToRotation(Vector2.right, dir);


        GameObject laser = Instantiate(laserPrefab, transform.position, rot);
        Laser laserScript = laser.GetComponent<Laser>();
        laserScript.Initialize(dir, laserDuration, fadeTime);
    }

    public void UpgradeWeapon()
    {
        laserWidth += 100f;
        damage += 2;
        Debug.Log($"레이저 굵기 굵기 증가.: {laserWidth:F2}, 데미지: {damage}");
    }
    
}
