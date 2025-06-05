using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class InventoryController : MonoBehaviour
{
    [SerializeField] public Inventory inventory;
    [SerializeField] public GameObject inventoryObject;
    [SerializeField] public GameObject[] weaponObjectReference;

    [SerializeField] public int equipedIndex;
    public bool hasMultipleWeapons;

    public void InitInventory(Inventory inv)
    {
        inventory = inv;
        weaponObjectReference = new GameObject[inventory.weaponSlots.Length];
        equipedIndex = 0;
        hasMultipleWeapons = false;
    }

    private void FixedUpdate()
    {
        ScrollWheelInput();
        ResetWeaponRendered();
    }

    public void AddWeapon(weapon w)
    {
        PlayerUI.instance.UpdateAmmoUI(w.currantClipSize, w.currantStockSize);

        if (inventory.weaponSlots[0].IsEmpty())
        {
            inventory.SetWeaponData(w, 0);
            SpawnWeapon(w,0);
            return;

        }
        if (!inventory.weaponSlots[0].IsEmpty() && inventory.weaponSlots[1].IsEmpty())
        {
            inventory.SetWeaponData(w, 1);
            SpawnWeapon(w, 1);
            hasMultipleWeapons = true;
            return;
        }
        else if (!inventory.weaponSlots[0].IsEmpty() && !inventory.weaponSlots[1].IsEmpty())
        {
            if (!hasMultipleWeapons) return;

            RemoveWeapon(equipedIndex);

            inventory.SetWeaponData(w, equipedIndex);
            SpawnWeapon(w, equipedIndex);
            return;
        }
    }

    public void RemoveWeapon(int index)
    {
        Destroy(weaponObjectReference[index].gameObject);
    }

    public void SpawnWeapon(weapon w, int index) 
    {
        GameObject g = Instantiate(w.prefab, inventoryObject.transform.position ,Quaternion.identity);
        g.transform.parent = inventoryObject.transform;
        weaponObjectReference[index] = g;

        ActivateWeapons();
    }

    public void ActivateWeapons()
    {
        for (int i = 0; i < weaponObjectReference.Length - 1; i++)
        {
            if (i == equipedIndex && weaponObjectReference[equipedIndex] != null)
            {
                weaponObjectReference[equipedIndex].SetActive(true);
            }
            else if (i != equipedIndex && weaponObjectReference[i] != null)
            {
                weaponObjectReference[equipedIndex].SetActive(false);
            }
        }
    }

    public void ScrollWheelInput()
    {
        if (hasMultipleWeapons)
        {
            //If has multiple weapons change equip index
            if (Input.mouseScrollDelta.y == 1)
            {
                if (equipedIndex >= weaponObjectReference.Length - 1)
                    return;

                equipedIndex++;

            }
            if (Input.mouseScrollDelta.y == -1)
            {
                if (equipedIndex == 0)
                    return;
                else if (equipedIndex < 0)
                    equipedIndex = 0;

                equipedIndex--;
            }
        }
    }

    public void ResetWeaponRendered()
    {
        if (weaponObjectReference[0] != null)
            weaponObjectReference[0].SetActive(false);

        if (weaponObjectReference[1] != null)
            weaponObjectReference[1].SetActive(false);

        if (weaponObjectReference[0] != null)
            weaponObjectReference[equipedIndex].SetActive(true);
    }

    }  
    
