using UnityEngine;
using System.Collections;
public class MidBoss : Enemy
{
    public float chargeSpeed = 8f;
    public float chargeCooldown = 5f;
    private bool isCharging = false;
    private float nextChargeTime;

    protected override void Update()
    {
        if (isCharging)
        {
            transform.position += (player.position - transform.position).normalized * chargeSpeed * Time.deltaTime;
        }
        else
        {
            base.Update(); // 일반 이동

            if (Time.time >= nextChargeTime)
            {
                StartCoroutine(ChargeRoutine());
                nextChargeTime = Time.time + chargeCooldown;
            }
        }
    }

    IEnumerator ChargeRoutine()
    {
        isCharging = true;
        yield return new WaitForSeconds(0.6f);
        isCharging = false;
    }
}
