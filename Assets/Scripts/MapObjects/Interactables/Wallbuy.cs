using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallbuy : Interactables
{
    [SerializeField] private weapon wData;

    public void BuyWeapon(PlayerController pC)
    {
        if (!hasBeenUsed)
        {
            pC.invController.AddWeapon(wData);
            //pC.invController.ResetWeaponRendered();
            hasBeenUsed = true;
            return;
        }
        else
        {
            //BuyAmmo
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerUI.instance.EnableInteractText(this);
            //check if inventory has this weapondata
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
