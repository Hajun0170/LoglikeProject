using UnityEngine;
public interface IWeapon
{
    WeaponType GetWeaponType();
    void UpgradeWeapon();
}