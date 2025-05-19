using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracadeController : MonoBehaviour
{
    [SerializeField] GameObject[] barricadeList;
    
    private void Start()
    {
        int numOfBarriers = GameObject.FindGameObjectsWithTag("Barricade").Length;
        barricadeList = new GameObject[numOfBarriers];

        barricadeList = GameObject.FindGameObjectsWithTag("Barricade");
    }

    public GameObject FindClosetBarrierToMe(Vector3 ePos)
    {
        GameObject closestBarrier = new GameObject();
        float shortestDist = 500;

        for(int i = 0; i < barricadeList.Length; i++)
        {
            float d = Vector3.Distance(barricadeList[i].transform.position, ePos);
            
            if(d < shortestDist)
            {
                closestBarrier = barricadeList[i];
                shortestDist = d;   
            }
        }


        return closestBarrier;
    }
}
