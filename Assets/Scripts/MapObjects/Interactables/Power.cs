using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : Interactables
{
    public void ActivatePower()
    {
        GameManager.instance.ActivatePower();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !GameManager.instance.isPowerOn)
        {
            PlayerUI.instance.EnablePowerText(gameObject.name);
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
