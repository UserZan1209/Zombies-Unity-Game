using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactables
{
    private enum DoorType
    {
        def,
        power
    }

    [SerializeField] private Animator animator;
    [SerializeField] private DoorType type;

    private void Start()
    {
        animator = GetComponent<Animator>();

        switch (type) 
        {
            case DoorType.def:
                requiresPower = false;
                break;
            case DoorType.power:
                requiresPower = true;
                break;
            default:
                Debug.Log("ERROR on a doortype");
                break;
        }

    }

    private void Update()
    {
        if (hasBeenUsed)
            return;

        if (requiresPower && !hasBeenUsed)
            CheckForPower();
    }

    public void UseDoor()
    {
        animator.SetTrigger("openDoor");
    }

    private void CheckForPower() 
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.isPowerOn)
            {
                UseDoor();
                hasBeenUsed = true;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenUsed)
            return;

        //interaction text
        if (other.gameObject.tag == "Player")
        {

            if (requiresPower)
            {
                PlayerUI.instance.NeedPowerText();
            }
            else
            {
                PlayerUI.instance.EnableInteractText(this);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hasBeenUsed)
            return;

        //interaction text
        if (other.gameObject.tag == "Player")
        {
            PlayerUI.instance.DisbaleInteractText();
        }

    }
}
