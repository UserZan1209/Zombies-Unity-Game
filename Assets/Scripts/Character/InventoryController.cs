using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class WeaponSlot
{
    public WeaponSlot()
    {
        wData = null;
        wController = null;
        weaponRef = null;
    }

    public weapon wData;
    public WeaponController wController;
    public GameObject weaponRef;

    public bool IsEmpty()
    {
        if (wData == null)
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
    [SerializeField] public int equipedIndex;
    public bool hasMultipleWeapons = false;

    public Inventory(int weaponSlotAmount)
    {
        weaponSlots = new WeaponSlot[weaponSlotAmount];
        equipedIndex = 0;
        Debug.Log(weaponSlots.Length);
    }

    public void AddWeapon(weapon w, GameObject obj)
    {

        if (weaponSlots[0].IsEmpty() == true)
        {
            weaponSlots[0].wData = w;
            weaponSlots[0].weaponRef = obj;
            equipedIndex = 0;
            Debug.Log("b " + weaponSlots.Length);
            return;

        }
        else if(weaponSlots[0].IsEmpty() == false)
        {
            weaponSlots[0].weaponRef.SetActive(false);
            
            weaponSlots[1].wData = w;
            weaponSlots[1].weaponRef = obj;
            equipedIndex = 1;
            hasMultipleWeapons = true;
            Debug.Log("c " + weaponSlots.Length);
            return;
        }
        else if(weaponSlots[0].IsEmpty() == false && weaponSlots[1].IsEmpty() == false) 
        {
            weaponSlots[equipedIndex].wData = w;
            weaponSlots[equipedIndex].weaponRef = obj;
            Debug.Log("a " + weaponSlots.Length);
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
