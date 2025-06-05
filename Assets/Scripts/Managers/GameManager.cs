using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Variables-and-References
    [HideInInspector] public static GameManager instance;

    [Header("References")]
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] public GameObject playerRef;
    [SerializeField] private GameObject lightManager;

    [Header("Game Settings")]
    [SerializeField] public int roundNum;
    [SerializeField] private int targetFrameCap;
    [SerializeField] private int nukePointAmount;
    [SerializeField] public bool isInstaKill = false;
    [SerializeField] public bool isDoublePoints = false;
    [SerializeField] public float powerUpTimer = 0.0f;

    [Header("Sound Settings")]
    [SerializeField] private AudioSource BGM;

    [Header("Map Settings")]
    [SerializeField] private Interactables[] powerDoors;
    [SerializeField] public Zone playerActiveZone;
    [SerializeField] public bool isPowerOn = false;
    [SerializeField] public bool gameEnded = false;

    [Header("Score Data")]
    [SerializeField] private int killCount;
    [SerializeField] private int headshotCount;
    [SerializeField] private int downCount;

    #endregion

    private void Start()
    {
        InitGameManager();
    }

    private void InitGameManager()
    {
        instance = this;
        BGM = GetComponent<AudioSource>();

        playerUI = PlayerUI.instance;

        //Disabled during testing
        targetFrameCap = 28;
        Application.targetFrameRate = targetFrameCap;

        roundNum = 1;

        FindPlayer();
    }

    private void Update()
    {
        PowerUpTimer();

        if(gameEnded && Input.anyKey)
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(0);
        }
            
    }

    public void Nuke()
    {
        int points = nukePointAmount;
        if (isDoublePoints)
            points = points * 2;

        playerRef.GetComponent<PlayerController>().IncreaseScore(points);
        
        //nuke UI vfx
    }

    private void PowerUpTimer()
    {
        if(powerUpTimer > 0.0f)
            powerUpTimer -= Time.deltaTime;
        else if(powerUpTimer <= 0.0f)
        {
            powerUpTimer = 0.0f;
            if(isInstaKill)
                isInstaKill = false;
            if(isDoublePoints)
                isDoublePoints = false;

            PowerUpController.Instance.DisableIcon();
        }
    }

    #region Reference-Functions

    private void FindPlayer()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        if (playerRef == null)
        {
            FindPlayer();
        }
    }

    public Zone GetPlayerZone()
    {
        return playerActiveZone;
    }
    #endregion

    #region Game-Functions
    public void IncreaseScore(int s)
    {
        playerRef.GetComponent<PlayerController>().IncreaseScore(s);
    }

    public void IncreaseKillCount()
    {
        killCount++;
    }

    public void IncreaseHeadshotCount()
    {
        headshotCount++;
    }

    public void IncreaseDownCount()
    {
        downCount++;
    }

    public void updateRound(int r)
    {
        roundNum = r;
    }

    public void ActivatePower()
    {
        isPowerOn = true;

        LightManager lm = lightManager.GetComponent<LightManager>();
        lm.Activate();
    }
    #endregion

    public void EndGame()
    {
        //save progress
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.4f;
        //Game over on UI - keep track of kills, downs, headshots
        PlayerUI.instance.OpenStatMenu(killCount, headshotCount, downCount);
        PlayerUI.instance.OpenGameOver(roundNum);
        gameEnded = true;
    }

    public void OpenStatMenu()
    {
        playerUI.OpenStatMenu(killCount, headshotCount, downCount);
    }
}
