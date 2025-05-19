using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    [Header("Map Settings")]
    [SerializeField] private Interactables[] powerDoors;
    [SerializeField] public Zone playerActiveZone;
    [SerializeField] public bool isPowerOn = false;
    #endregion

    private void Start()
    {
        InitGameManager();
    }

    private void InitGameManager()
    {
        instance = this;
        playerUI = PlayerUI.instance;

        //Disabled during testing
        targetFrameCap = 29;
        Application.targetFrameRate = targetFrameCap;

        roundNum = 1;

        FindPlayer();
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

    public void updateRound(int r)
    {
        roundNum = r;
    }

    public void ActivatePower()
    {
        isPowerOn = true;

        LightManager lm = lightManager.GetComponent<LightManager>();
        lm.Activate();

        if (powerDoors[0] == null)
        {
            return;
        }
        powerDoors[0].GetComponent<Door>().UseDoor();


    }
    #endregion

    public void EndGame()
    {
        //save progress
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}
