using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoxPool_", menuName = "ScriptableObjects/Box/Pool", order = 1)]
public class WeaponPool : ScriptableObject
{
    [SerializeField] public weapon[] boxPool;

    public weapon GetWeapon(int index)
    {
        if (index > boxPool.Length)
            return null;

        return boxPool[index];
    }
}
