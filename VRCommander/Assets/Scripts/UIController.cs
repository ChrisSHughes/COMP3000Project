using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    int locationInt = 1;

    public PlayerBuildController buildController;
    public PlayerUnitController unitController;


    private void Start()
    {

    }

    public void SetConstructionYard()
    {
        Debug.Log("Construction Yard Set");
        buildController.SetBuilding(1);
    }

    public void SetPowerPlant()
    {
        Debug.Log("Power Plant Set");
        buildController.SetBuilding(2);
    }

    public void SetBarracks()
    {
        Debug.Log("Barracks Set");
        buildController.SetBuilding(3);
    }

    public void SetWarFactory()
    {
        Debug.Log("War Factory Set");
        buildController.SetBuilding(4);
    }

    public void TrainTank()
    {
        GameObject tank = Instantiate(Resources.Load<GameObject>("Prefabs/GameObjects/Units/BlueTank"));
        tank.transform.position = new Vector3(locationInt, 0, 1);
        tank.tag = "BlueUnit";
        locationInt++;
    }

    public void TrainMachineGunner()
    {
        GameObject tank = Instantiate(Resources.Load<GameObject>("Prefabs/GameObjects/Units/BlueMachineGunner"));
        tank.transform.position = new Vector3(locationInt, 0, 1);
        tank.tag = "BlueUnit";
        locationInt++;
    }
}
