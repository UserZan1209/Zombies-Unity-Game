using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour 
{
    [SerializeField] private PowerUpType Type;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PowerUpController.Instance.ActivatePowerUp(Type);
            Destroy(gameObject);
        }
    }
}
