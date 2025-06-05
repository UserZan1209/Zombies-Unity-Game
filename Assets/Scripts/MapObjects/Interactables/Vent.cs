using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : Interactables
{
    [SerializeField]private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenVent()
    {
        anim.SetTrigger("openVent");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (hasBeenUsed)
                return;

            PlayerUI.instance.EnablePowerText(GetInteractableName());
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
