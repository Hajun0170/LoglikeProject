using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
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
        titleText.text = "체력 회복\nHP를 모두 회복합니다.";
    }
    else if ((cardType == CardType.NewWeapon || cardType == CardType.UpgradeWeapon) && weaponType != WeaponType.None)
    {
        if (weaponInfoDict.TryGetValue(weaponType, out WeaponInfo info))
        {
            if (cardType == CardType.NewWeapon)
            {
                titleText.text = $"무기\n{info.displayName}\n<size=70%>{info.description}</size>";
            }
            else if (cardType == CardType.UpgradeWeapon)
            {
                    if (weaponType == WeaponType.Ranged)
                    {
                        titleText.text = $"무기 강화\n{info.displayName}\n<size=70%> 파이어볼의 투사체의 개수가 1개 늘어납니다.</size>";

                    }
                    else if (weaponType == WeaponType.ChainLightning)
                    {
                        titleText.text = $"무기 강화\n{info.displayName}\n<size=70%> 체인 라이트닝의 연쇄 공격이 1회 증가합니다.</size>";
                    }
                    else if (weaponType == WeaponType.SpinningBlade)
                    {
                        titleText.text = $"무기 강화\n{info.displayName}\n<size=70%> 회전 칼날의 칼날이 1개 증가합니다.</size>";
                    }
                    else
                    {
                       titleText.text = $"무기 강화\n{info.displayName}\n<size=70%> 레이저의 폭과 데미지가 증가합니다.</size>";   
                    }
            }
        }
        else
        {
            titleText.text = "알 수 없는 무기";
        }
    }

    // 버튼 클릭 시 선택
    selectButton.onClick.AddListener(() => manager.SelectCard(cardType, weaponType));

    Debug.Log($"카드 설정됨: {cardType}, {weaponType}");
}

    private static readonly Dictionary<WeaponType, WeaponInfo> weaponInfoDict = new()
{
    { WeaponType.Ranged, new WeaponInfo("[파이어볼]", "\n가장 가까운 적을 향해 투사체를 발사합니다") },
    { WeaponType.SpinningBlade, new WeaponInfo("[회전 칼날]", "\n플레이어 주변을 회전하며 적에게 피해를 줍니다") },
    { WeaponType.Laser, new WeaponInfo("[레이저]", "\n가장 가까운 적을 향해 강력한 광선을 발사하고 후방의 적에게도 피해를 줍니다.") },
    { WeaponType.ChainLightning, new WeaponInfo("[체인 라이트닝]", "\n적에게 번개를 쏘고 주변 적에게 연쇄적인 피해를 입힙니다.") }
};
}
