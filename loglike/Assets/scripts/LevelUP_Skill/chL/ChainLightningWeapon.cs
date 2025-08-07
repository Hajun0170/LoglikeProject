using UnityEngine;
using System.Linq;

public class ChainLightningWeapon : MonoBehaviour, IWeapon
{
    public GameObject pfLightningProjectile;
    public float fFireRange = 8f;
    public int iChainCount = 2;
    public int iDamage = 10;
    public float fCooldown = 1.5f;

    private float fTimer = 0f;

    public WeaponType GetWeaponType() => WeaponType.ChainLightning;

    void Update()
    {
        fTimer += Time.deltaTime;
        if (fTimer >= fCooldown)
        {
            Fire();
            fTimer = 0f;
        }
    }

    void Fire()
    {
        GameObject[] aEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (aEnemies.Length == 0) return;

        GameObject tTarget = aEnemies
            .Where(e => Vector2.Distance(transform.position, e.transform.position) <= fFireRange)
            .OrderBy(e => Vector2.Distance(transform.position, e.transform.position))
            .FirstOrDefault();

        if (tTarget != null)
        {
            SpawnLightning(tTarget.transform, iChainCount);
        }
    }

    public void SpawnLightning(Transform tTarget, int iRemainingChains)
    {
        GameObject goLightning = Instantiate(pfLightningProjectile, transform.position, Quaternion.identity);
        LightningProjectile lightning = goLightning.GetComponent<LightningProjectile>();
        lightning.Initialize(tTarget, iDamage, iRemainingChains, this);
    }

    public void UpgradeWeapon()
    {
        iChainCount++; // 업그레이드 시 연쇄 수 증가
        Debug.Log($"⚡ 체인 라이트닝 강화됨: 연쇄 횟수 {iChainCount}");
    }
    
}
