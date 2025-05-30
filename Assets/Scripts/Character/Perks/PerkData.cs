using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PerkType
{
    QR,
    DT,
    JG,
    SC
}

[CreateAssetMenu(fileName = "PerkData", menuName = "ScriptableObjects/Inventory/Perk", order = 2)]
public class PerkData : ScriptableObject
{
    public string perkName;
    public Sprite perkIcon;
    public PerkType perkType;
    public Color perkColor;
}
