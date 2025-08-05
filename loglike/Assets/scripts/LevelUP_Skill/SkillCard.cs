using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCard : MonoBehaviour
{
    public TMP_Text titleText;
    public Button selectButton;

    private CardType cardType;
    private WeaponType weaponType;
    private LevelUpManager manager;

    public void Setup(CardType type, WeaponType wType, LevelUpManager m)
    {
        cardType = type;
        weaponType = wType;
        manager = m;

        // 카드 텍스트 설정
        if (cardType == CardType.Heal)  
        {
            titleText.text = "체력 회복";
        }
        else if ((cardType == CardType.NewWeapon || cardType == CardType.UpgradeWeapon) && weaponType != WeaponType.None)
        {
            string label = cardType == CardType.NewWeapon ? "무기" : "강화";
            titleText.text = $"{label}\n {weaponType}";
        }
        else
        {
            titleText.text = "알 수 없음";
            Debug.LogWarning($"❓ 예기치 않은 카드 조합: {cardType}, {weaponType}");
        }

        // 버튼 클릭 시 선택
        selectButton.onClick.AddListener(() => manager.SelectCard(cardType, weaponType));

        Debug.Log($"🃏 카드 설정됨: {cardType}, {weaponType}");
    }
}
