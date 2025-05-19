using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI instance;
    public GameObject playerScoreText;
    public GameObject interactText;
    public GameObject roundText;

    [SerializeField] private Image attack1;
    [SerializeField] private Image attack2;
    [SerializeField] private Image Flash;

    void Start()
    {
        instance = this;
        roundText.GetComponent<TextMeshProUGUI>().text = 1.ToString();
        interactText.SetActive(false);
        attack1.enabled = false;
        attack2.enabled = false;
        //Flash.enabled = false;  
    }

    public void Update()
    {
        if (Flash.enabled && Flash.color.a != 0)
        {
            Color flashCol = new Color(Flash.color.r, Flash.color.g, Flash.color.b, Flash.color.a - Time.deltaTime);
            Flash.color = flashCol; 
        }
        else
        {
            Color flashCol = new Color(Flash.color.r, Flash.color.g, Flash.color.b, 255);
            Flash.color = flashCol;
            Flash.enabled = false;

        }
    }

    public void EnableInteractText(Interactables iData)
    {
        interactText.SetActive(true);
        string text = "Press F to buy " + iData.GetInteractableName() + "  $" + iData.GetInteractablePrice().ToString();
        interactText.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void NeedPowerText()
    {
        interactText.SetActive(true);
        string text = "This requires the power to be turned on";
        interactText.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void EnablePowerText(string interactableName)
    {
        interactText.SetActive(true);
        string text = "Press F to use " + interactableName;
        interactText.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void UpdateRoundText(int r)
    {
        roundText.GetComponent<TextMeshProUGUI>().text = r.ToString();
    }

    public void UpdateScoreText(int s)
    {
        playerScoreText.GetComponent<TextMeshProUGUI>().text = s.ToString();
    }

    public void DisbaleInteractText()
    {
        interactText.SetActive(false);
    }

    public void DamageIndicator(float playerHealth)
    {
        if (attack1.enabled && playerHealth <= 49)
        {
            attack2.enabled = true;
        }
        else
        {
            attack2.enabled = false;
        }

        if(playerHealth < 100)
            attack1.enabled = true;
        else
        {
            attack1.enabled = false;
        }

        if (playerHealth <= 0)
        {
            attack1.enabled = false;
            attack2.enabled = false;
        }
    }

    public void EnableFlash()
    {
        Flash.enabled = true;
    }
}
