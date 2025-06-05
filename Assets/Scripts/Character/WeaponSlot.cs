using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponSlot
{
    public bool isUsed;
    public weapon wData;
    public WeaponController wController;
    public GameObject weaponRef;

    public WeaponSlot()
    {
        isUsed = false;
        wData = new weapon();
        wController = new WeaponController();
        weaponRef = null;
    }

    public bool IsEmpty()
    {
        if (isUsed == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
