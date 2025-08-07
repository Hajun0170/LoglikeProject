using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.EventSystems; // 이걸 꼭 추가
public class LevelUpManager : MonoBehaviour
{
    public GameObject levelUpPanel;
    public bool IsCardActive => levelUpPanel.activeSelf;
    public Transform cardContainer;
    public GameObject cardPrefab;

    private PlayerStats playerStats;
    private WeaponManager weaponManager;



    void Start()
    {
       playerStats = FindObjectOfType<PlayerStats>();

    weaponManager = FindObjectOfType<WeaponManager>();
    //levelUpPanel.SetActive(false);

    StartCoroutine(DelayedShowInitialCards());
    }

   public void ShowCards(bool isInitial = false)
{
    Time.timeScale = 0f;
    levelUpPanel.SetActive(true);

    EventSystem.current.SetSelectedGameObject(null); // 선택된 UI 제거

    foreach (Transform child in cardContainer)
        Destroy(child.gameObject);

    List<(CardType, WeaponType)> cardOptions = GenerateCardOptions(isInitial);

    foreach (var (type, wType) in cardOptions)
    {
        GameObject card = Instantiate(cardPrefab, cardContainer);
        Debug.Log($"✅ 카드 생성됨: {type} / {wType}");
        card.GetComponent<SkillCard>().Setup(type, wType, this);
    }
}

    public void SelectCard(CardType cardType, WeaponType wType)
    {
        switch (cardType)
        {
            case CardType.NewWeapon:
                weaponManager.AddWeapon(wType); break;
            case CardType.UpgradeWeapon:
                weaponManager.UpgradeWeapon(wType); break;
            case CardType.Heal:
                playerStats.RestoreHealth(1); break;
        }

       levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private List<(CardType, WeaponType)> GenerateCardOptions(bool isInitial)
    {
        List<(CardType, WeaponType)> result = new();
         var allTypes = System.Enum.GetValues(typeof(WeaponType)).Cast<WeaponType>()
                    .Where(w => w != WeaponType.None); // ✅ None 무기 제거

       if (isInitial)
    {
        foreach (WeaponType w in allTypes)
            result.Add((CardType.NewWeapon, w));
        result = result.OrderBy(_ => Random.value).ToList();
    }
         else
    {
        foreach (WeaponType w in allTypes)
        {
            if (!weaponManager.HasWeapon(w)) result.Add((CardType.NewWeapon, w));
            else result.Add((CardType.UpgradeWeapon, w));
        }


             if (playerStats.currentHealth < playerStats.maxHealth)
            result.Add((CardType.Heal, WeaponType.None));
    }

    return result.OrderBy(_ => Random.value).Take(3).ToList();
}

IEnumerator DelayedShowInitialCards()
{
    yield return new WaitForSecondsRealtime(0.2f);  // ⚠ 반드시 WaitForSecondsRealtime 사용
    ShowCards(isInitial: true);
}

    public void ShowLevelUpChoices(bool isInitial = false)
    {
        Debug.Log("📦 스킬카드 UI 호출됨");
        ShowCards(isInitial); // ✅ 카드 생성 호출 추가

    }


    

}
