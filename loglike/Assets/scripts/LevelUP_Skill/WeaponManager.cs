using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    private Dictionary<WeaponType, IWeapon> equippedWeapons = new();


    public List<GameObject> weaponPrefabs; // Inspector에 등록
    public Transform weaponHolder;
public void AddWeapon(WeaponType type)
{
    if (equippedWeapons.ContainsKey(type)) return;

     GameObject prefab = weaponPrefabs.Find(w =>
        {
            var provider = w.GetComponent<IWeapon>();
            return provider != null && provider.GetWeaponType() == type;
        });

    if (prefab == null)
    {
        Debug.LogError($"❌ {type} 타입의 무기 프리팹을 찾을 수 없습니다.");
        return;
    }

    GameObject obj = Instantiate(prefab, weaponHolder);

    IWeapon weaponInstance = obj.GetComponent<IWeapon>();
    if (weaponInstance == null)
    {
        Debug.LogError($"❌ {type} 무기에서 IUpgradableWeapon을 찾을 수 없습니다.");
        return;
    }

    equippedWeapons[type] = weaponInstance;

    Debug.Log($"무기 획득: {type}");
}

 public void UpgradeWeapon(WeaponType type)
    {
        if (equippedWeapons.TryGetValue(type, out var weapon))
        {
            var method = weapon.GetType().GetMethod("UpgradeWeapon");
            if (method != null)
            {
                method.Invoke(weapon, null);
                Debug.Log($"무기 강화: {type}");
            }
            else
            {
                Debug.LogWarning($"⚠️ {type} 무기에는 UpgradeWeapon() 메서드가 없습니다.");
            }
        }
    }

  public bool HasWeapon(WeaponType type) => equippedWeapons.ContainsKey(type);
}

