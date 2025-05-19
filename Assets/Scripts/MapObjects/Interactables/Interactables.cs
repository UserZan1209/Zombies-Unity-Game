using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum InteractableTypes 
{
    Perk,
    Box,
    Debris,
    Door,
    PowerDoor,
    power,
    wallbuy
}

/*
 * Interactables will encapsulate everything in the enviroment the player can interact with
 * Doors,Power,Perks,Box,WallWeapons.
*/
[System.Serializable]
public class Interactables : MonoBehaviour
{
    #region interactable-vars
    [Header("Interactable Data")]
    [SerializeField] private string interactableName;
    [SerializeField] public InteractableTypes interactableType;
    [SerializeField] private int requiredPoints;
    [SerializeField] private Zone connectedZone;
    [SerializeField] public bool requiresPower;
    [SerializeField] public bool hasBeenUsed;
    #endregion

    #region functionallity
    public void ActivateInteractable(PlayerController pC)
    {
        if (hasBeenUsed) 
        {
            return;
        }

        if(requiresPower && !GameManager.instance.isPowerOn)
        {
            return;
        }

        //Remove points for use of interactable
        if (pC != null)
        {
            if (pC.GetScore() < requiredPoints)
            {
                print("not enough");
                return;
            }


            #region interactable-actions
            switch (interactableType)
            {
                case InteractableTypes.Perk:
                    gameObject.GetComponent<Perk>().UsePerk(pC);
                    break;
                case InteractableTypes.Box:
                    gameObject.GetComponent<Box>().UseBox();
                    break;
                case InteractableTypes.Door:
                    gameObject.GetComponent<Door>().UseDoor();
                    break;
                case InteractableTypes.Debris:
                    gameObject.GetComponent<Debris>().RemoveDebris();
                    break;
                case InteractableTypes.power:
                    gameObject.GetComponent<Power>().ActivatePower();
                    break;
                case InteractableTypes.wallbuy:
                    gameObject.GetComponent<Wallbuy>().BuyWeapon(pC);
                    break;
                default:
                    Debug.Log("INVALID-INTERACTABLE-TYPE");
                    break;
            }




            hasBeenUsed = true;
            #endregion

            pC.DecreaseScore(requiredPoints);

        }
        else
        {
            Debug.Log("[WARNING] Score system bypassed");
        }

        //If door leads to new area it will enable spawning in there
        if (connectedZone != null && connectedZone.isActive == false)
        {
            connectedZone.isActive = true;
        }


    }
    #endregion

    #region Get-Set-Functions
    public void SetInteractableName(string newName)
    {
        interactableName = newName;
    }

    public string GetInteractableName()
    {
        return interactableName;
    }

    public void SetInteractablePrice(int newPoints)
    {
        requiredPoints = newPoints;
    }

    public int GetInteractablePrice()
    {
        return requiredPoints;
    }
    #endregion


}
