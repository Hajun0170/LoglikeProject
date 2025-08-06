using UnityEngine;

public class LaserWeapon : MonoBehaviour
{
    public GameObject laserPrefab;
    public float fireRate = 1f;
    public float laserDuration = 0.5f;
    public float fadeTime = 0.2f;

    private float nextFireTime = 0f;
    private TargetTracker targetTracker;

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
}
