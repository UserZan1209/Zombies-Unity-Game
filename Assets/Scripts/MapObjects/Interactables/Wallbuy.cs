using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallbuy : Interactables
{
    [SerializeField] private weapon wData;

    public void BuyWeapon(PlayerController pC)
    {
        GameObject g = Instantiate(wData.prefab, pC.invObject.transform.position, Quaternion.identity);

        g.transform.parent = pC.invObject.transform;
        g.gameObject.SetActive(true);
        pC.inventory.AddWeapon(wData, g);

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
