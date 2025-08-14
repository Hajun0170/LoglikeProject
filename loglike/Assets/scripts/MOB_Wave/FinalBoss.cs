using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class FinalBoss : Enemy
{
    
    public float chargeSpeed = 6f;
    public float chargeCooldown = 4f;
    public float shootCooldown = 2.5f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    private float nextChargeTime;
    private float nextShootTime;
    private bool isCharging = false;

    private Slider bossSlider; // 슬라이더 참조

    public void Init(Slider slider)
    {
        bossSlider = slider;
        bossSlider.gameObject.SetActive(true);
        bossSlider.maxValue = maxHP;
        bossSlider.value = maxHP;   
    }

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

            if (Time.time >= nextShootTime)
            {
                FireProjectile();
                nextShootTime = Time.time + shootCooldown;
            }
        }

        if (bossSlider != null)
        {
            bossSlider.value = currentHP;
        }
    }

    IEnumerator ChargeRoutine()
    {
        isCharging = true;
        yield return new WaitForSeconds(0.8f);
        isCharging = false;
    }

    void FireProjectile()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = dir * projectileSpeed;
    }

    protected override void Die()
    {
        base.Die();

        // 클리어 UI 출력
        PauseManager_Clear pauseManager = FindObjectOfType<PauseManager_Clear>();
        if (pauseManager != null)
        {
            pauseManager.SetPause(true);
        }

        // 슬라이더 숨김
        if (bossSlider != null)
        {
            bossSlider.gameObject.SetActive(false);
        }
    }
}
