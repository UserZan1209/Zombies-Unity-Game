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
    [SerializeField] public Character chr;
    [SerializeField] public Inventory inv;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private InputActionReference moveIA;
    [SerializeField] private InputActionReference cameraMoveIA;
 
    [Header("Variables")]
    [SerializeField] private float playerHealth;
    [SerializeField] private float playerHealthMAX;
    [SerializeField] private float speedRunning;
    [SerializeField] private float speedWalking;
    [SerializeField] private int playerScore;
    [SerializeField] public bool isDowned;
    [Tooltip("Enables God-Mode")]
    [SerializeField] private bool GDM;
    [SerializeField] private bool grounded;
    [SerializeField] private bool isRunning;

    [SerializeField] private float RegenerationRate;
    private float verticalRotation = 0f;
    [SerializeField] private RaycastHit[] groundHits = new RaycastHit[8];

    [SerializeField] public bool hasQR, hasJG, hasDT, hasSC;

    [Header("Constants")]
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float vSpeed = 0.0f;


    //Velocity.
    private Vector3 Velocity
    {
        //Getter.
        get => rb.velocity;
        //Setter.
        set => rb.velocity = value;
    }

    public Rigidbody GetRB()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        return rb; 
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        chr = GetComponent<Character>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        

        cameraTransform = Camera.main.transform;

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerHealthMAX = 100;
        playerHealth = playerHealthMAX;
        playerScore = 500;

        hasQR = false;
        hasJG = false;
        hasDT = false;
        hasSC = false;

        isDowned = false;
        grounded = true;

        speedWalking = 5.0f;
        speedRunning = 9.0f;
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
                    zController.TakeDamage(35);
                    GameManager.instance.IncreaseScore(50);
                }
            }
        }


        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            //aim
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            armAnimator.SetTrigger("hasKnifed");
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2.5f))
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
                        Debug.Log("A");
                        GameManager.instance.IncreaseScore(10);
                    }
                    else 
                    {
                        Debug.Log("B");
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

            //transform.position += moveDir * s * Time.deltaTime;

            Vector3 moveVector = -transform.forward * moveInput.x + transform.right * moveInput.y;
            rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z) * s;

        }
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void IncreaseScore(int points)
    {
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
            //trigger Downed or GameOver
            if (!hasQR)
            {
                GameManager.instance.EndGame();
            }

            Downed();
            hasDT = false;
            hasJG = false;
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
        playerHealth = playerHealthMAX;

        isDowned = false;
    }

    public int GetScore()
    {
        return playerScore;
    }

    public void AddPerk(PerkType pT)
    {
        switch (pT) 
        {
            case PerkType.QR:
                Debug.Log("QR");
                break;
            case PerkType.JG:
                Debug.Log("JG");
                break;
            case PerkType.SC:
                Debug.Log("SC");
                break;
            case PerkType.DT:
                Debug.Log("DT");
                break;
        }

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

