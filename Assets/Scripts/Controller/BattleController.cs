﻿using UnityEngine;
using System.Collections;

public class BattleController : StateMachine
{
    public GameObject soldierPrefab;
    public CameraController cameraController;
    public Board board;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public Point pos;
    public Unit currentUnit;
    public Tile currentTile { get { return board.GetTile(pos); } }

    void Start()
    {
        ChangeState<InitBattleState>();
    }
}