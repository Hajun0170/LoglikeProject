using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : Enemy
{
    public enum BossType { Mid, Final }
    public BossType bossType;

    public GameObject healthBarUI;
    public Slider healthSlider;

    public override void OnReuse()
    {
        base.OnReuse();
        ShowHealthBar();
        UpdateHealthUI();
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        UpdateHealthUI();
    }

    protected override void Die()
    {
        HideHealthBar();

        if (bossType == BossType.Mid)
            FindObjectOfType<SpawnManager>().OnBossDefeated();
        else if (bossType == BossType.Final)
            FindObjectOfType<GameManager>().OnGameClear();

        base.Die();
    }

    void ShowHealthBar()
    {
        if (healthBarUI != null)
            healthBarUI.SetActive(true);
    }

    void HideHealthBar()
    {
        if (healthBarUI != null)
            healthBarUI.SetActive(false);
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = (float)currentHP / maxHP;
    }
}
