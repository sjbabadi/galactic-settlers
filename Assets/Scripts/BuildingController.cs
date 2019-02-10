using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    //need references
    [SerializeField] private GameState gs;
    [SerializeField] private UIController uiController;
    [SerializeField] private Building[] buildings; //contains refs to the different building prefabs
    [SerializeField] private Map map;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Soldier soldier;

    private Building selectedBuilding; // this is the building that will be built when AddBuilding is eventually called


    private Vector3 mouseButtonDown;
    private Vector3 mouseButtonUp;
    private bool buildSoldier;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseButtonDown = cameraController.transform.position;
        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseButtonUp = cameraController.transform.position;
            if (Vector3.Distance(mouseButtonDown, mouseButtonUp) < 0.02f)
            {
                if (selectedBuilding != null)
                {
                    InteractWithBoard();
                }
                else if (buildSoldier)
                {
                    InteractWithBoard_Soldier();
                }
            }
        }
    }

    void InteractWithBoard()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) //out keyword is so that we are passing by reference and not by value
        {
            Vector3 gridPosition = map.CalculateGridPosition(hit.point); //this line calls the method that rounds the position values

            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && map.isPositionAvailable(gridPosition)) //if positioin is empty
            {
                if (gs.Money >= selectedBuilding.cost) //and we have enough money
                {
                    gs.Money -= selectedBuilding.cost;
                    uiController.UpdatePlayerData();
                    gs.buildingCounts[selectedBuilding.id]++; //remember that the id's map to the array index that the building corresponds to
                    map.AddBuilding(selectedBuilding, gridPosition);
                }
            }

        }
    }

    void InteractWithBoard_Soldier()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) //out keyword is so that we are passing by reference and not by value
        {
            Vector3 gridPosition = map.CalculateGridPosition(hit.point); //this line calls the method that rounds the position values

            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && map.isPositionAvailable(gridPosition)) //if positioin is empty
            {
                if (gs.Money >= soldier.cost && gs.PopulationCurrent < gs.PopulationMax) //and we have enough money
                {
                    gs.Money -= soldier.cost;
                    gs.PopulationCurrent++;
                    uiController.UpdatePlayerData();
                    map.AddPiece(soldier.gameObject, gridPosition);
                    map.AddBuilding(selectedBuilding, gridPosition);
                }
            }

        }
    }

    //builder argument correlates to an index in the building array
    public void EnableBuilder(int building)
    {
        selectedBuilding = buildings[building]; // now selectedBuilding will contain a ref to a prefab
        Debug.Log("Selected Building: " + selectedBuilding.buildingName);
    }

    public void BuildSoldier()
    {
        selectedBuilding = null;
        buildSoldier = true;

    }
}
