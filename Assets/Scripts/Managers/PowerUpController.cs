using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpController : MonoBehaviour
{
    [HideInInspector] public static PowerUpController Instance;
    [SerializeField] private GameObject IconContainer;
    [SerializeField] private Image instaIcon;
    [SerializeField] private TextMeshProUGUI doubleIcon;
    
    private void Start()
    {
        Instance = this;
        instaIcon.enabled = false;
        IconContainer.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.O))
            ActivatePowerUp(PowerUpType.Instakill);
    }

    public void ActivatePowerUp(PowerUpType p)
    {
        switch (p) 
        {
            case PowerUpType.Instakill:
                IconContainer.SetActive(true);
                instaIcon.enabled = true;
                GameManager.instance.isInstaKill = true;
                GameManager.instance.powerUpTimer = 30.0f;
                break;
            case PowerUpType.Nuke:
                ZombieSpawner.Instance.KillAll();
                GameManager.instance.Nuke();
                break;
            case PowerUpType.DoublePoints:
                IconContainer.SetActive(true);
                doubleIcon.enabled = true;
                GameManager.instance.isDoublePoints = true;
                GameManager.instance.powerUpTimer = 30.0f;
                break;
            default:
                break;
        }

    }

    public void DisableIcon()
    {
        instaIcon.enabled = false;
        doubleIcon.enabled = false;
        IconContainer.SetActive(false);
    }
}
