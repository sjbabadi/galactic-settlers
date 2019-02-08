using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
     private GameState gs;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI dataText;
    // Start is called before the first frame update
    void Start()
    {
        gs = FindObjectOfType<GameState>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTurnCount()
    {
        turnText.text = string.Format("Turn: {0}", gs.Turn);
    }

    public void UpdatePlayerData()
    {
        dataText.text = string.Format("Jobs: {0}/{1}\n" +
            "Money: {2}\n" +
            "Pop: {3}/{4}\n" +
            "Food: {5}",
            gs.UnitsCurrent, gs.UnitsMax, gs.Money, gs.PopulationCurrent, gs.PopulationMax, gs.Food);
    }
}
