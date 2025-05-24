using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// offhand is saved for melee weapons
public enum WeaponTypes
{
    Pistol,
    AkimboPistol,
    SubmachineGun,
    AutomaticRifle,
    Rifle,
    Offhand
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/Inventory/Weapon", order = 1)]
public class weapon : ScriptableObject
{
    public string weaponName;
    public string weaponDescription;
    public GameObject prefab;
    public WeaponTypes weaponType;
    public Material camoMaterial;

    public int bulletDamage;
    public int maxClipSize;
    public int maxStockSize;
    public int currantClipSize;
    public int currantStockSize;

    public void Fire()
    {

    }

    public void Reload()
    {

    }


}
