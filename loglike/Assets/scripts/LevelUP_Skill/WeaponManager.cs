using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    private Dictionary<WeaponType, Weapon> equippedWeapons = new();

    public List<GameObject> weaponPrefabs; // Inspector에 등록
    public Transform weaponHolder;

    public void AddWeapon(WeaponType type)
    {
        if (equippedWeapons.ContainsKey(type)) return;

        GameObject prefab = weaponPrefabs.Find(w => w.GetComponent<Weapon>().weaponType == type);
        GameObject obj = Instantiate(prefab, weaponHolder);
        Weapon weapon = obj.GetComponent<Weapon>();
        equippedWeapons[type] = weapon;

        Debug.Log($"무기 획득: {type}");
    }

    public void UpgradeWeapon(WeaponType type)
    {
        if (equippedWeapons.TryGetValue(type, out var weapon))
        {
            weapon.Upgrade();
            Debug.Log($"무기 강화: {type} → Lv.{weapon.level}");
        }
    }

    public bool HasWeapon(WeaponType type) => equippedWeapons.ContainsKey(type);
    public bool IsMaxLevel(WeaponType type) => HasWeapon(type) && equippedWeapons[type].level >= 5;
}
