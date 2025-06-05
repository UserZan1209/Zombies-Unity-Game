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
    public GameObject iconPrefab;
    public Image[] perkIcons;
    public GameObject perkIconContainer;
    public float perkOffsetX;
    public float perkOffsetY;
    public float perkIconOffset;

    [SerializeField] private GameObject gameOverContainer;
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI headshotText;
    [SerializeField] private TextMeshProUGUI downText;
    [SerializeField] private TextMeshProUGUI endRoundText;
    [SerializeField] private TextMeshProUGUI magazineText;
    [SerializeField] private TextMeshProUGUI stockText;

    [SerializeField] private Image attack1;
    [SerializeField] private Image attack2;
    [SerializeField] private Image attack3;
    [SerializeField] private Image Flash;

    void Start()
    {
        instance = this;
        roundText.GetComponent<TextMeshProUGUI>().text = 1.ToString();
        interactText.SetActive(false);
        attack1.enabled = false;
        attack2.enabled = false;
        attack3.enabled = false;
        gameOverContainer.SetActive(false);
        //Flash.enabled = false;  
        perkOffsetX = 60.0f;
        perkOffsetY = 50.0f;
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

    public void EnableTakeText(string interactableName)
    {
        interactText.SetActive(true);
        string text = "Press F to take " + interactableName;
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
        {
            attack1.enabled = true;
        }
        else
        {
            attack1.enabled = false;
        }

        if (playerHealth < 30)
        {
            attack3.enabled = true;
        }
        else
        {
            attack3.enabled = false;
        }

        if (playerHealth <= 0)
        {
            DisableAllDamageIndicators();
        }
    }

    public void EnableFlash()
    {
        Flash.enabled = true;
    }

    public void DisableAllDamageIndicators()
    {
        attack1.enabled = false;
        attack2.enabled = false;
        attack3.enabled = false;
    }

    public void OpenStatMenu(int k, int h, int d)
    {
        gameOverContainer.SetActive(true);

        killText.text = k.ToString();
        downText.text = d.ToString();
        headshotText.text = h.ToString();
    }

    public void UpdateAmmoUI(int mag, int stock)
    {
        magazineText.text = mag.ToString();
        stockText.text = stock.ToString();
    }

    public void OpenGameOver(int r)
    {
        endRoundText.text = r.ToString();
    }

    public void AddPerk(PerkData[] pD)
    {
        int f = 0;
        for (int i = 0; i < pD.Length; i++) 
        {
            if (pD[i] != null)
                f++;
        }

        perkIcons = new Image[f];

        for(int i = 0; i < perkIcons.Length; i++)
        {
            GameObject img = Instantiate(iconPrefab);
            img.transform.parent = perkIconContainer.transform;

            Vector3 pos = new Vector3(perkIconContainer.transform.position.x + (perkOffsetX * i), perkIconContainer.transform.position.y + perkOffsetY, 0);
            img.transform.position = pos;

            perkIcons[i] = img.GetComponent<Image>();
            perkIcons[i].color = pD[i].perkColor;
        }
    }
}
