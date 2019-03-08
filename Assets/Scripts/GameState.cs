using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public int Turn { get; set;  }

    public int Money { get; set; }

    public int Food { get; set; }

    public int Population { get; set; }

    public int BaseHealth { get; set; }

    public int Units { get; set; }

    public int MaxPop { get; set; }

    private HUDController HUDController;

    private void Start()
    {
        HUDController = FindObjectOfType<HUDController>();
    }

    public void EndTurn()
    {
        Turn++;
        //HUDController.UpdateStats();
    }
}
