using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkMachine : Interactables
{
    public PerkData perkData;
    //Give perk() - check if already has it
    public void UsePerk(PlayerController pC)
    {
        if (hasBeenUsed || !GameManager.instance.isPowerOn)
            return;

        pC.AddPerk(perkData);
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
