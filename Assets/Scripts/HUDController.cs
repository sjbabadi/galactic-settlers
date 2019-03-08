using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    //[SerializeField] private GameState gs
    [SerializeField] private Text turnText;
    [SerializeField] private Text baseHealthText;
    [SerializeField] private Text moneyText;
    [SerializeField] private Text unitsText;
    [SerializeField] private Text foodText;
    [SerializeField] private Text maxPopText;

    // Used to toggle the stats panel between a displayed and hidden state
    [SerializeField] private GameObject statsPanel;
    private RectTransform rt;
    private bool statsDisplayed = false;
    private Vector3 statsClose, statsOpen;

    private void Start()
    {
        rt = statsPanel.GetComponent<RectTransform>();
        statsClose = rt.transform.localPosition;
        statsOpen = rt.transform.localPosition + new Vector3(-340,0,0);
    }

    public void ShowStatsPanel() {
        if (!statsDisplayed)
        {
            rt.localPosition = statsOpen;
            statsDisplayed = true;
        }
    }

    public void HideStatsPanel() {
        if (statsDisplayed)
        {
            rt.localPosition = statsClose;
            statsDisplayed = false;
        }
    }

    // Updates all text based on the values recoreded in the GameState object. Everything commented out as GameState object does not yet exist.
    public void UpdateStats() {
        //turnText.text = string.Format("Turn: {0}", gs.Turn);
        //baseHealthText.text = string.Format("Base Health: {0}", gs.BaseHealth);
        //moneyText.text = string.Format("Money: {0}", gs.Money);
        //unitstext = string.Format("Units: {0}", gs.Units);
        //foodText.text = string.Format("Food: {0}", gs.Food);
        //maxPopText.text = string.Format("Max. Pop.: {0}", gs.MaxPop);
    }
}
