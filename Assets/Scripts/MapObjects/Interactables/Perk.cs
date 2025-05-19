using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PerkType
{
    QR,
    DT,
    JG,
    SC
}

public class Perk : Interactables
{
    public PerkType perkType;

    //Give perk() - check if already has it
    public void UsePerk(PlayerController pC)
    {
        if (hasBeenUsed || !GameManager.instance.isPowerOn)
            return;

        pC.AddPerk(perkType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (hasBeenUsed)
                return;

            if (GameManager.instance.isPowerOn)
            {
                PlayerUI.instance.EnableInteractText(this);
            }
            else
            {
                PlayerUI.instance.NeedPowerText();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerUI.instance.DisbaleInteractText();
        }

    }
}
