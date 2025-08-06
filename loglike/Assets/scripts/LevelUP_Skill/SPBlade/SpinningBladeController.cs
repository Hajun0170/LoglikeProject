using UnityEngine;

public class SpinningBladeController : MonoBehaviour
{
    public GameObject pfBlade;
    public int iBladeCount = 2;
    public float fRadius = 1.5f;
    public float fRotationSpeed = 180f; // degrees per second

    private Transform[] aBlades;

    void Start()
    {
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

    void Update()
    {
        for (int i = 0; i < aBlades.Length; i++)
        {
            // 회전 각도 계산
            float fAngle = fRotationSpeed * Time.time + (360f / iBladeCount) * i;
            float fRad = fAngle * Mathf.Deg2Rad;

            // 위치 업데이트
            Vector3 vOffset = new Vector3(Mathf.Cos(fRad), Mathf.Sin(fRad), 0) * fRadius;
            aBlades[i].localPosition = vOffset;
        }
    }
}
