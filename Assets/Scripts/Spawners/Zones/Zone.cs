using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Zone : MonoBehaviour
{
    #region Zone-Vars
    [Header("Zone Data")]
    [SerializeField] public bool isActive;
    [SerializeField] private GameObject spawnContainer;

    [HideInInspector] public GameObject[] spawnPoints;
    #endregion

    private void Start()
    {
        initZone();
    }

    #region Zone-Functions
    private void initZone()
    {
        //Stores all the zones spawnpoints in an array
        int length = spawnContainer.transform.childCount;
        spawnPoints = new GameObject[length];

        for (int i = 0; i < length; i++)
        {
            spawnPoints[i] = spawnContainer.transform.GetChild(i).gameObject;
        }
    }

    //returns a random spawn within the zone
    public Transform GetRandomSpawn()
    {
        int r = Random.Range(0, spawnContainer.transform.childCount);
        Transform newSpawn = spawnContainer.transform.GetChild(r).transform;

        return newSpawn;
    }
    #endregion

    #region Collision-Functions
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Enables zone if a player enters it regardless of it being opened as a failsafe
            if(!isActive)
                isActive = true;

            GameManager.instance.playerActiveZone = this;
        }
    }
    #endregion
}
