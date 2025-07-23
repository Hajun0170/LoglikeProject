using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public GameObject projectilePrefab;
    public float fireRate = 1f;
    public float projectileSpeed = 10f;
}
