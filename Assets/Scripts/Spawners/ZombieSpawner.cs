using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    #region Variables-and-References
    [SerializeField] public static ZombieSpawner Instance;

    [Header("Zombie Types")]
    [SerializeField] private GameObject basicZombiePrefab;
    [HideInInspector] private GameObject[] zPool;

    [Header("Spawning References")]
    [SerializeField] private GameObject spawnPointContainer;
    [SerializeField] private GameObject mapZoneContainer;
    [SerializeField] private Zone[] mapZones;
    [SerializeField] private Zone spawnZone;

    [Header("Game Info")]
    [SerializeField] private int roundNumber = 0;
    [SerializeField] private int zombieCount = 0;
    #endregion

    private void Start()
    {
        init();
        StartNextRound(roundNumber);
    }

    private void init()
    {
        Instance = this;

        #region define-mapzones
        int length = mapZoneContainer.transform.childCount;
        mapZones = new Zone[length];

        for (int i = 0; i < length; i++)
        {
            mapZones[i] = mapZoneContainer.transform.GetChild(i).gameObject.GetComponent<Zone>();
        }

        //Mapzone[0] refers to the spawn room
        spawnZone = mapZones[0];
        zPool = new GameObject[23];
        #endregion


        //Play starting sound
        roundNumber = 0;
    }

    private void StartNextRound(int prevRound)
    {
        // Update Round
        roundNumber++;
        if(GameManager.instance != null) 
            GameManager.instance.updateRound(roundNumber);

        if (PlayerUI.instance != null)
            PlayerUI.instance.UpdateRoundText(roundNumber);

        //Round change sound


        //clear previous round zombies
        //may changne to a respawn function
        for (int i = 0; i < zPool.Length; i++) 
        {
            Destroy(zPool[i]);
        }


        //calculate the amount to spawn
        int zAmount = (prevRound + 1) + 4;
        if (zAmount > 24)
        {
            zAmount = 24;
        }

        while (zombieCount < zAmount)
        {
            //Add spawnrate

            SpawnZombie();
            zombieCount++;
        }

    }

    #region ZSpawner-Functions
    public void SpawnZombie()
    {
        if (zombieCount >= (roundNumber + 1) + 4)
            return;

        //Decide on a spawnzone
        if (GameManager.instance != null)
        {
            // Change to pick from enabled zones
            spawnZone = GameManager.instance.GetPlayerZone();
        }
        else
        {
            spawnZone = mapZones[0];
        }

        GameObject z = Instantiate(basicZombiePrefab, spawnZone.GetRandomSpawn());
        z.GetComponent<EController>().zmSpawner = this;

        zPool[zombieCount] = z;
    }

    public void DecreaseZCount()
    {
        zombieCount--;

        if (zombieCount <= 0)
        {
            StartNextRound(roundNumber);
        }
    }

    public void ZombieDespawned()
    {
        SpawnZombie();
    }

    #endregion


    public int GetZCount()
    {
        return zombieCount;
    }

    public void KillAll()
    {
        for(int i = 0; i < zPool.Length; i++)
        {
            if(zPool[i] != null)
            {
                Destroy(zPool[i]);
            }
        }

        zombieCount = 0;
        StartNextRound(roundNumber);
    }

}
