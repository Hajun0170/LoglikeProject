using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class LightningProjectile : MonoBehaviour
{
    public float fSpeed = 10f;
    private Transform tTarget;
    private int iDamage;
    private int iRemainingChains;
    private ChainLightningWeapon tWeaponRef;
    private List<GameObject> lHitEnemies = new List<GameObject>();

    public void Initialize(Transform target, int damage, int chainCount, ChainLightningWeapon weaponRef)
    {
        tTarget = target;
        iDamage = damage;
        iRemainingChains = chainCount;
        tWeaponRef = weaponRef;
    }

    void Update()
    {
        if (tTarget == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 vDir = (tTarget.position - transform.position).normalized;
        transform.position += vDir * fSpeed * Time.deltaTime;

        if (Vector2.Distance(transform.position, tTarget.position) < 0.1f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        Enemy enemy = tTarget.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(iDamage);
            lHitEnemies.Add(tTarget.gameObject);
        }

        if (iRemainingChains > 1)
        {
            GameObject[] aEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject tNextTarget = aEnemies
                .Where(e => !lHitEnemies.Contains(e) && e != tTarget.gameObject)
                .OrderBy(e => Vector2.Distance(tTarget.position, e.transform.position))
                .FirstOrDefault(e => Vector2.Distance(tTarget.position, e.transform.position) <= tWeaponRef.fFireRange);

            if (tNextTarget != null)
            {
                // [중요] 발사 위치를 플레이어가 아닌 "방금 맞은 적 위치"로 설정
                GameObject goNextLightning = Instantiate(tWeaponRef.pfLightningProjectile, tTarget.position, Quaternion.identity);
                LightningProjectile lightning = goNextLightning.GetComponent<LightningProjectile>();
                lightning.Initialize(tNextTarget.transform, iDamage, iRemainingChains - 1, tWeaponRef);
                lightning.SetHitList(lHitEnemies); // 이전 히트 리스트 전달
            }
        }

        Destroy(gameObject);
    }

public void SetHitList(List<GameObject> previousHits)
{
    lHitEnemies = new List<GameObject>(previousHits);
}

}
