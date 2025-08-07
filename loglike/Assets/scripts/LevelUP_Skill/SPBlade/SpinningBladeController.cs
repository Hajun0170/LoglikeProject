using UnityEngine;

public class SpinningBladeController : MonoBehaviour, IWeapon
{
    public GameObject pfBlade;
    public int iBladeCount = 2;
    public float fRadius = 1.5f;
    public float fRotationSpeed = 180f; // degrees per second

    private Transform[] aBlades;
    public WeaponType GetWeaponType() => WeaponType.SpinningBlade;
    void Start()
    {
        SpawnBlades();
    }

    void Update()
    {
        RotateBlades();
    }

    void SpawnBlades()
    {
        // 기존 블레이드 제거
        if (aBlades != null)
        {
            foreach (Transform blade in aBlades)
            {
                if (blade != null)
                    Destroy(blade.gameObject);
            }
        }

        aBlades = new Transform[iBladeCount];
        float fAngleStep = 360f / iBladeCount;

        for (int i = 0; i < iBladeCount; i++)
        {
            GameObject goBlade = Instantiate(pfBlade, transform);
            float fAngle = i * fAngleStep * Mathf.Deg2Rad;
            Vector3 vOffset = new Vector3(Mathf.Cos(fAngle), Mathf.Sin(fAngle), 0) * fRadius;
            goBlade.transform.localPosition = vOffset;
            aBlades[i] = goBlade.transform;
        }
    }

    void RotateBlades()
    {
        for (int i = 0; i < aBlades.Length; i++)
        {
            float fAngle = fRotationSpeed * Time.time + (360f / iBladeCount) * i;
            float fRad = fAngle * Mathf.Deg2Rad;
            Vector3 vOffset = new Vector3(Mathf.Cos(fRad), Mathf.Sin(fRad), 0) * fRadius;
            aBlades[i].localPosition = vOffset;
        }
    }
    
     public void UpgradeWeapon()
    {
        iBladeCount++;
        SpawnBlades(); // 새로 배치
        Debug.Log("현재 회전 칼날 수: {iBladeCount}");
    }
}
