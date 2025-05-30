using InfimaGames.LowPolyShooterPack;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Referemces")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator armAnimator;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] public InventoryController invController;
    [SerializeField] public Inventory inventory;
    [SerializeField] private const int WEAPON_SLOT_COUNT = 2;
    [SerializeField] public GameObject invObject;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private InputActionReference moveIA;
    [SerializeField] private InputActionReference cameraMoeIA;
 
    [Header("Variables")]
    [SerializeField] private float playerHealth;
    [SerializeField] private float playerHealthMAX;
    [SerializeField] private float speedRunning;
    [SerializeField] private float speedWalking;
    [SerializeField] private float slideForce;
    [SerializeField] private int playerScore;
    [SerializeField] public bool isDowned;
    [Tooltip("Enables God-Mode")]
    [SerializeField] private bool GDM;
    [SerializeField] private bool grounded;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isCrouched;
    [SerializeField] private bool isProned;
    [SerializeField] private Vector3 proneOffsetVector;

    [SerializeField] private float RegenerationRate;
    private float verticalRotation = 0f;
    [SerializeField] private RaycastHit[] groundHits = new RaycastHit[8];

    [SerializeField] public bool hasQR, hasJG, hasDT, hasSC;

    [Header("Constants")]
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float vSpeed = 0.0f;


    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        capsuleCollider = GetComponent<CapsuleCollider>();
        invController = gameObject.AddComponent<InventoryController>();
        invController.inventoryObject = invObject;
        invController.inventory = new Inventory(WEAPON_SLOT_COUNT);

        inventory = invController.inventory;    

        cameraTransform = Camera.main.transform;

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerHealthMAX = 100;
        playerHealth = playerHealthMAX;
        playerScore = 50000;

        hasQR = false;
        hasJG = false;
        hasDT = false;
        hasSC = false;

        isDowned = false;
        grounded = true;
        isRunning = false;
        isCrouched = false;
        isProned = false;

        speedWalking = 5.0f;
        speedRunning = 8.0f;
    }

    private void Awake()
    {

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.tag == "Zombie")
                {
                    GameObject zm = hit.collider.gameObject;
                    EController zController = zm.GetComponent<EController>();

                    if (!zController.isActive)
                        return;

                    PlayerController pController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();


                    //needs changing to weapon damage


                    if (hasDT)
                    {
                        zController.TakeDamage(20);
                        GameManager.instance.IncreaseScore(20);
                        return;
                    }
                    else
                    {
                        zController.TakeDamage(10);
                        GameManager.instance.IncreaseScore(10);
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Alpha1)) 
        {
            if (inventory.weaponSlots[0].wData == null)
                return;

            inventory.SwitchWeapon();
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            if (inventory.weaponSlots[1].wData == null)
                return;

            inventory.SwitchWeapon();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isRunning)
                return;

            isRunning = true;
            armAnimator.SetBool("isRunning", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (!isRunning)
                return;

            isRunning = false;
            armAnimator.SetBool("isRunning", false);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            isCrouched = !isCrouched;

            if(isCrouched)
            {
                capsuleCollider.height = 1.0f;
                if (isRunning)
                {
                    rb.AddForce(cameraTransform.forward * slideForce, ForceMode.Impulse);
                }
            }
            else
            {
                capsuleCollider.height = 2.0f;
                isProned = false;
            }
                
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isProned = !isProned;

            if (isProned)
                capsuleCollider.height = 0.4f;
            else
            {
                capsuleCollider.height = 2.0f;
                isCrouched = false;
            }
                

        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            armAnimator.SetBool("isAiming", true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            armAnimator.SetBool("isAiming", false);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            armAnimator.SetTrigger("hasKnifed");
            RaycastHit hit;

            //Knife
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3.5f))
            {
                if (hit.collider.gameObject.tag == "Zombie")
                {

                    GameObject zm = hit.collider.gameObject;
                    EController zController = zm.GetComponent<EController>();

                    if (!zController.isActive)
                        return;

                    PlayerController pController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                    
                    if (zController.GetHealth() - 100 > 0) 
                    {
                        GameManager.instance.IncreaseScore(10);
                    }
                    else 
                    {
                        GameManager.instance.IncreaseScore(100);
                    }

                    zController.TakeDamage(100);

                    
                }
            }
        }

        #region debug-functions
        if (Input.GetKeyUp(KeyCode.M)) 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            LoadMainMenu();
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            if (isDowned)
            {
                Revive();
            }
            else
            {
                TakeDamage(50);
            }
        }

        if (Input.GetKeyUp(KeyCode.Q)) 
        {
            PlayerUI.instance.EnableFlash();
        }
        #endregion

        #region Passive-Function
        RegenerateHealth();
        #endregion


    }

    private void FixedUpdate()
    {
        Movement();

        if (grounded && Input.GetKeyUp(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 10f, ForceMode.VelocityChange);
            grounded = false;
        }
    }

    public void Movement()
    {
        //Get Movement Input
        Vector2 moveInput = moveIA.action.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(moveInput.x,0,moveInput.y);
       
        if (moveDir != Vector3.zero && grounded) 
        {
            float s = 0;
            if (isRunning)
            {
                s = speedRunning;
            }
            else if (!isRunning)
            {
                s = speedWalking;
            }

            if (isCrouched)
            {
                s = speedRunning / 2.0f;
            }
            else if (!isCrouched && !isRunning)
            {
                s = speedWalking;
            }

            if (isProned)
            {
                s = speedRunning / 4.0f;
            }
            else if (!isProned && !isRunning)
            {
                s = speedWalking;
            }

            //transform.position += moveDir * s * Time.deltaTime;

            Vector3 moveVector = -transform.forward * moveInput.x + transform.right * moveInput.y;
            rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z) * s;

        }

        //Fix for a velocity issue causing player to gain large speeds
        if (rb.velocity.y > 30)
            rb.velocity = Vector3.zero;
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void IncreaseScore(int points)
    {
        if (GameManager.instance.isDoublePoints)
            points = points * 2;

        playerScore += points;
        PlayerUI.instance.UpdateScoreText(playerScore);
    }

    public void DecreaseScore(int points)
    {
        playerScore -= points;
        PlayerUI.instance.UpdateScoreText(playerScore);
    }

    private void RegenerateHealth()
    {
        if(playerHealth < playerHealthMAX)
        {
            playerHealth += Time.deltaTime * RegenerationRate;
            PlayerUI.instance.DamageIndicator(playerHealth);
        }
    }

    public void UpdateMaxHealth(int newMax)
    {
        playerHealthMAX = newMax;
    }

    public void TakeDamage(int damage)
    {
        if (isDowned || GDM)
            return;

        playerHealth -= damage;
        PlayerUI.instance.DamageIndicator(playerHealth);

        if(playerHealth <= 0 && !isDowned)
        {
            armAnimator.SetTrigger("tDeath");
            PlayerUI.instance.DisableAllDamageIndicators();
            //trigger Downed or GameOver
            if (!hasQR)
            {
                GameManager.instance.EndGame();
            }

            Downed();
            hasDT = false;
            hasJG = false;
            ChangeMaxHealth();
            hasSC = false;
            hasQR = false;

            isDowned = true;

            //update UI

        }
    }

    public void Downed()
    {
        capsuleCollider.height = 0.0f;
 
        playerHealth = 0;
    }

    public void Revive()
    {
        capsuleCollider.height = 1.5f;
        armAnimator.SetTrigger("tRevive");
        playerHealth = playerHealthMAX;

        isDowned = false;
    }

    public int GetScore()
    {
        return playerScore;
    }

    public void AddPerk(PerkData pT)
    {
        inventory.AddPerkToList(pT);

        PlayerUI.instance.AddPerk(inventory.activePerks);

        switch (pT.perkType) 
        {
            case PerkType.QR:
                hasQR = true;
                break;
            case PerkType.SC:
                hasSC = true;
                break;
            case PerkType.DT:
                hasDT = true;
                break;
            case PerkType.JG:
                hasJG = true;
                ChangeMaxHealth();
                break;
            default:
                Debug.Log("No valid perk type");
                break;
        }
    }

    private void ChangeMaxHealth()
    {
        if (hasJG)
        {
            playerHealthMAX = 200.0f;
        }
        else
        {
            playerHealthMAX = 100.0f;
            
        }
        playerHealth = playerHealthMAX;
    }

    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;
        vSpeed = 0;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "interactable")
        {
            Interactables iData = other.gameObject.GetComponent<Interactables>();
            if (iData.hasBeenUsed)
                return;


            //wait for input to use interactable
            if (Input.GetKeyUp(KeyCode.F))
            {
                iData.ActivateInteractable(this);
                PlayerUI.instance.DisbaleInteractText();
            }
        }


    }

}

