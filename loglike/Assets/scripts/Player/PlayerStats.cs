using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("Exp/Level")]
    public int currentExp = 0;
    public int level = 1;
    public int expToNextLevel = 100;
    public Slider expBar;

    [Header("HP")]
    public int maxHealth = 5;
    public int currentHealth = 5;
    public float invincibleTime = 3f;
    private bool isInvincible = false;
    private LevelUpManager levelUpManager;

    [Header("UI")]
    public HealthUIManager uiManager;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        UpdateExpUI();
        uiManager.UpdateHearts(currentHealth);

        // 게임 시작 시 무기 선택 카드 띄우기
        FindObjectOfType<LevelUpManager>().ShowLevelUpChoices(true);

        
    }

    void Awake()
{
    levelUpManager = FindObjectOfType<LevelUpManager>();
}

    // 경험치 추가
    public void AddExp(int amount)
    {
        currentExp += amount;

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
        }

        UpdateExpUI();
    }

    // 레벨업 처리
    void LevelUp()
    {
        level++;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.2f);
        Debug.Log($"레벨 {level} 달성!");

        if (levelUpManager != null)
        levelUpManager.ShowLevelUpChoices(false);
    else
        Debug.LogError("LevelUpManager가 연결되지 않았ek!");
    }

    void UpdateExpUI()
    {
        if (expBar != null)
        {
            expBar.maxValue = expToNextLevel;
            expBar.value = currentExp;
        }
    }

    // 체력 회복
    public void RestoreHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"체력 회복됨: {currentHealth}/{maxHealth}");
        uiManager.UpdateHearts(currentHealth);
    }

    // 데미지 처리
    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);
        uiManager.UpdateHearts(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    void Die()
    {
        Debug.Log("악 쥬금ㅠ");
        PauseManager_DEAD pauseManager = FindObjectOfType<PauseManager_DEAD>();
    if (pauseManager != null && !pauseManager.IsPaused())
    {
        pauseManager.TogglePause();
    }
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        float elapsed = 0f;
        while (elapsed < invincibleTime)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.2f);
            elapsed += 0.2f;
        }

        sr.enabled = true;
        isInvincible = false;
    }

    // 외부에서 호출할 때 쓸 수 있는 힐 함수
    public void Heal(int amount)
    {
        RestoreHealth(amount);
    }
}
