using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallbuy : Interactables
{
    [SerializeField] private GameObject weaponPrefab;

    public void BuyWeapon(PlayerController pC)
    {
        GameObject g = Instantiate(weaponPrefab, pC.inv.transform);

        g.transform.parent = pC.inv.transform;
        g.gameObject.SetActive(true);

        WeaponBehaviour wB = weaponPrefab.GetComponent<WeaponBehaviour>();


        Destroy(pC.inv.weapons[pC.inv.equippedIndex].gameObject);
        pC.inv.weapons[pC.inv.equippedIndex] = wB;
        pC.inv.equipped = wB;

        Debug.Log(pC.chr.equippedWeapon);
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
