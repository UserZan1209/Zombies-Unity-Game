using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    [SerializeField] private GameObject boardContainer;
    [SerializeField] private GameObject[] boards;

    [SerializeField] private GameObject entryObject;

    private void Start()
    {
        boards = new GameObject[boardContainer.transform.childCount];

        for (int i = 0; i < boardContainer.transform.childCount; i++)
        {
            boards[i] = boardContainer.transform.GetChild(i).gameObject;
        }
    }

    private void Update()
    {
        
    }

    public void RemoveBarriers()
    {
        for (int i = 0; i < boards.Length; i++)
        {
            boards[i].GetComponent<Rigidbody>().isKinematic = false;
            boards[i].GetComponent<Rigidbody>().useGravity = true;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Zombie")
        {
            other.gameObject.transform.position = entryObject.transform.position;

            EController c = other.GetComponent<EController>();
            EMovement m = other.GetComponent<EMovement>();
            c.hasEnteredMap = true;
            m.EnableNavMesh();
        }
    }
}
