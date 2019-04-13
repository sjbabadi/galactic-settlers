using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    // Building stats
    public float health = 100;
    public int cost = 100;
    public Turn owner;
    public Buildings buildingType;
    public bool used = false;
}
