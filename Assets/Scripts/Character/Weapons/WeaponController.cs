using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    public void ActivateWeapon()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateWeapon()
    {
        gameObject.SetActive(false);
    }

}
