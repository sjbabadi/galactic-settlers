using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameState gs;
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

    //player controls
    [SerializeField] private GameObject playerControls;
    private RectTransform pc;
    private bool controlsDisplayed = false;
    private Vector3 controlsOpen, controlsClose;

    private void Start()
    {
        rt = statsPanel.GetComponent<RectTransform>();
        statsClose = rt.transform.localPosition;
        statsOpen = rt.transform.localPosition + new Vector3(-340,0,0);

        pc = playerControls.GetComponent<RectTransform>();
        controlsClose = pc.transform.localPosition;
        controlsOpen = pc.transform.localPosition + new Vector3(0, +460, 0);

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

    public void MoveControlsPanel()
    {
        if (!controlsDisplayed)
        {
            pc.localPosition = controlsOpen;
            controlsDisplayed = true;
        } else
        {
            pc.localPosition = controlsClose;
            controlsDisplayed = false;
        }
    }


    // Updates all text based on the values recoreded in the GameState Object
    public void UpdateStatText() {
        turnText.text = string.Format("Turn: {0}", gs.gameTurn);
        baseHealthText.text = string.Format("Base Health: {0}", gs.BaseHealth[(int)Turn.Player]);
        moneyText.text = string.Format("Money: {0}", gs.Money[(int)Turn.Player]);
        unitsText.text = string.Format("Units: {0}/{1}", gs.Units[(int)Turn.Player], gs.UnitMax[(int)Turn.Player]);
        foodText.text = string.Format("Food: {0}", gs.Food[(int)Turn.Player]);
        maxPopText.text = string.Format("Pop: {0}/{1}", gs.Population[(int)Turn.Player], gs.MaxPop[(int)Turn.Player]);
    }
}
