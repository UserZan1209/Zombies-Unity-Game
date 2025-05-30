using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class WeaponSlot
{
    public WeaponSlot()
    {
        isUsed = false;
        wData = null;
        wController = null;
        weaponRef = null;
    }

    public bool isUsed;
    public weapon wData;
    public WeaponController wController;
    public GameObject weaponRef;

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

[System.Serializable]
public class Inventory
{
    public GameObject weaponContainer;
    [SerializeField] public WeaponSlot[] weaponSlots;
    [SerializeField] public PerkData[] activePerks;
    [SerializeField] public int equipedIndex;
    public bool hasMultipleWeapons = false;

    public Inventory(int weaponSlotAmount)
    {
        weaponSlots = new WeaponSlot[weaponSlotAmount];
        activePerks = new PerkData[4];
        equipedIndex = 0;
        Debug.Log(weaponSlots.Length);
    }

    public void AddWeapon(weapon w, GameObject obj)
    {
        /*        if (weaponSlots[0] == null)
                    return;*/

        Debug.Log(weaponSlots[0].IsEmpty());

        if (weaponSlots[0].IsEmpty() == true)
        {
            weaponSlots[0].wData = w;
            weaponSlots[0].weaponRef = obj;
            equipedIndex = 0;

            weaponSlots[0].isUsed = true;
            return;

        }
        if(weaponSlots[0].IsEmpty() == false && weaponSlots[1].IsEmpty() == true)
        {
            weaponSlots[0].weaponRef.SetActive(false);
            
            weaponSlots[1].wData = w;
            weaponSlots[1].weaponRef = obj;
            equipedIndex = 1;
            weaponSlots[1].isUsed = true;
            hasMultipleWeapons = true;
            Debug.Log("c " + weaponSlots.Length);
            return;
        }
        if(weaponSlots[0].IsEmpty() == false && weaponSlots[1].IsEmpty() == false) 
        {
            GameObject.Destroy(weaponSlots[equipedIndex].weaponRef.gameObject);

            weaponSlots[equipedIndex].wData = w;
            weaponSlots[equipedIndex].weaponRef = obj;
            return;
        }



        //SwitchWeapon();
    }

    public void ChangeEquipIndex(float input)
    {

        if (Input.mouseScrollDelta.y == 1)
        {
            if (equipedIndex >= weaponSlots.Length-1)
                return;

            equipedIndex++;

            Debug.Log(equipedIndex);

        }
        if (Input.mouseScrollDelta.y == -1)
        {
            if (equipedIndex == 0)
                return;
            else if (equipedIndex < 0)
                equipedIndex = 0;

            equipedIndex--;

            Debug.Log(equipedIndex);
        }

        if (hasMultipleWeapons)
        {
            for (int i = 0; i < weaponContainer.transform.childCount; i++)
            {
                if (weaponSlots[i].weaponRef == null)
                    return;

                if(i == equipedIndex)
                {
                    weaponSlots[i].weaponRef.SetActive(true);
                }
                else
                {
                    weaponSlots[i].weaponRef.SetActive(false);
                }
            }
        }
    }

    public void RemoveWeapon()
    {
    
    }

    public void SwitchWeapon()
    {


        for(int i = 0; i < weaponSlots.Length; i++)
        {
            if(i != equipedIndex && weaponSlots[i].weaponRef != null)
            {
                weaponSlots[i].weaponRef.SetActive(false);
            }
            else
            {
                weaponSlots[i].weaponRef.SetActive(true);
            }
        }    
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

[System.Serializable]
public class InventoryController : MonoBehaviour
{
    [SerializeField] public Inventory inventory;
    [SerializeField] public GameObject inventoryObject;

    private void Start()
    {
        inventory.weaponContainer = inventoryObject;
    }

    private void FixedUpdate()
    {
        ScrollWheelInput();

    }

    private void ScrollWheelInput()
    {
        if (Input.mouseScrollDelta.y == 0)
            return;

        inventory.ChangeEquipIndex(Input.mouseScrollDelta.y);
/*        if (inventory.weaponSlots[inventory.equipedIndex].weaponRef != null)
        {
            inventory.SwitchWeapon();
        }
        else
        {
            return;
        }*/

    }
}
