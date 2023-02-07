using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public BuildController buildController;


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
}
