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
        dataText.text = string.Format("Money: {2}\n" +
            "Population: {3}/{4}\n" +
            "Food: {5}"\n+
            "Military Units: {0}/{1}\n",
            gs.UnitsCurrent, gs.UnitsMax, (int)gs.Money, (int)gs.PopulationCurrent, gs.PopulationMax, (int)gs.Food);
    }
}
