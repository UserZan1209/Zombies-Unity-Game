using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Interactables
{
    public void UseBox()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerUI.instance.EnableInteractText(this);
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
