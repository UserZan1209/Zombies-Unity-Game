using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Interactables
{
    [SerializeField] private WeaponPool bP;
    [SerializeField] private GameObject weaponSlot;
    [SerializeField] private float resetTimer = 0.0f;

    [SerializeField] private GameObject tempWeapon;
    [SerializeField] private weapon storedWeapon;

    private void Update()
    {
        if(resetTimer > 0.0f)
            resetTimer -= Time.deltaTime;
        if(resetTimer <= 0.0f && hasBeenUsed)
        {
            if(weaponSlot.transform.childCount != 0)
                Destroy(weaponSlot.transform.GetChild(0).gameObject);
            
            hasBeenUsed = false;
            resetTimer = 0.0f;
        }
    }

    public void UseBox(PlayerController pC)
    {
        if(hasBeenUsed) return;

        WeaponSlot[] wS = pC.inventory.weaponSlots;
        int r = Random.Range(0, bP.boxPool.Length);
        storedWeapon = bP.GetWeapon(r);

        //[WARNGING] Causes crashing.
/*
        if (wS[0].wData != null) 
        {
            for (int i = 0; i < wS.Length; i++)
            {
                if (storedWeapon == wS[i].wData)
                {
                    Debug.Log("reroll");
                    UseBox(pC);
                }
            }
        }
*/

        tempWeapon = Instantiate(bP.GetWeapon(r).prefab, weaponSlot.transform.position, weaponSlot.transform.rotation);

        tempWeapon.transform.parent = weaponSlot.transform;
        tempWeapon.GetComponent<WeaponController>().enabled = false;

        PlayerUI.instance.EnableTakeText(storedWeapon.weaponName);

        resetTimer += 15.0f;
        SetInteractablePrice(0);
    }

    public void TakeWeapon(PlayerController pC)
    {
        if (resetTimer > 14.0f || tempWeapon == null) return;

        pC.invController.AddWeapon(storedWeapon);
        //pC.invController.ResetWeaponRendered();

        PlayerUI.instance.DisbaleInteractText();

        Destroy(tempWeapon);

        hasBeenUsed = false;
        resetTimer = 0.0f;
        SetInteractablePrice(950);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!hasBeenUsed)
            {
                PlayerUI.instance.EnableInteractText(this);
            }
            if (hasBeenUsed)
            {
                if (storedWeapon == null) return;

                PlayerUI.instance.EnableTakeText(storedWeapon.weaponName);
            }


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerUI.instance.DisbaleInteractText();
        }

    }
}
