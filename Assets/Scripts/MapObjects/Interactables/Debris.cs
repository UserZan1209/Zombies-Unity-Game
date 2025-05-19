using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : Interactables
{
    private void Start()
    {
        requiresPower = false;
    }

    public void RemoveDebris()
    {
        Destroy(this.gameObject);
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
