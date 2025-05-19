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

    [SerializeField] private DoorType type;

    private void Start()
    {
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

    public void UseDoor()
    {
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && requiresPower)
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
        if (other.gameObject.tag == "Player")
        {
            PlayerUI.instance.DisbaleInteractText();
        }

    }
}
