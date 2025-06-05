using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Inventory
{
    [SerializeField] public WeaponSlot[] weaponSlots;
    [SerializeField] public PerkData[] activePerks;


    public Inventory(int weaponSlotAmount)
    {
        weaponSlots = new WeaponSlot[weaponSlotAmount];
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            weaponSlots[i] = new WeaponSlot();
        }
        activePerks = new PerkData[4];
    }


    public void SetWeaponData(weapon w, int index)
    {
        weaponSlots[index].wData = w;
        weaponSlots[index].weaponRef = w.prefab;

        weaponSlots[index].isUsed = true;
    }

    public void AddPerkToList(PerkData pD)
    {
        for (int i = 0; i < activePerks.Length; i++)
        {
            if (activePerks[i] == null)
            {
                activePerks[i] = pD;
                return;
            }
            else
            {
                Debug.Log("No room");
            }
        }
    }
}
