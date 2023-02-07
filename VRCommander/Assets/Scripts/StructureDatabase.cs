using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureDatabase : MonoBehaviour
{

    public List<Structure> redStructures = new List<Structure>();
    public List<Structure> blueStructures = new List<Structure>();
    
    public List<GameObject> redStructures2 = new List<GameObject>();
    public List<GameObject> blueStructures2 = new List<GameObject>();

    public List<Structure> ghostRedStructures = new List<Structure>();
    public List<Structure> ghostBlueStructures = new List<Structure>();

    public List<GameObject> ghostBlueStructures2 = new List<GameObject>();


    public void Awake()
    {
        BuildRedStructureList();
        BuildBlueStructureList();
        BuildRedGhostStructureList();
        BuildBlueGhostStructureList();

        for (int i = 0; i < blueStructures.Count; i++)
        {

            Debug.Log("red stuff");
            Debug.Log(redStructures[i].id + " , " + redStructures[i].name + " , " + redStructures[i].building);
            Debug.Log("blue stuff");
            Debug.Log(blueStructures[i].id + " , " + blueStructures[i].name + " , " + blueStructures[i].building);
        }
    }

    public void BuildRedStructureList()
    {
        redStructures = new List<Structure>()
        {
            new Structure(0, null, null),

            new Structure(1, "Construction Yard", Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Solid Building/Red Construction Yard")),

            new Structure(2, "Power Plant", Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Solid Building/Red PowerPlant")),

            new Structure(3, "Barracks", Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Solid Building/Red Barracks")),

            new Structure(4, "War Factory", Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Solid Building/Red War Factory"))

        };
    }

    public void BuildBlueStructureList()
    {
        blueStructures = new List<Structure>()
        {
            new Structure(0, null, null),

            new Structure(1, "Construction Yard",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Solid Building/Blue Construction Yard")),

            new Structure(2, "Power Plant",Resources.Load<GameObject>("BluePowerPlant")),

            new Structure(3, "Barracks",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Solid Building/Blue Barracks")),

            new Structure(4, "War Factory",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Solid Building/Blue War Factory"))

        };
    }

    public void BuildRedGhostStructureList()
    {
        ghostRedStructures = new List<Structure>()
        {
            new Structure(0, null, null),

            new Structure(1, "Construction Yard",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Ghost Building/Red Construction Yard (Ghost)")),

            new Structure(2, "Power Plant",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Ghost Building/Red Power Plant (Ghost)")),

            new Structure(3, "Barracks",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Ghost Building/Red Barracks (Ghost)")),

            new Structure(4, "War Factory",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Ghost Building/Red War Factory (Ghost)"))

        };
    }

    public void BuildBlueGhostStructureList()
    {
        ghostBlueStructures = new List<Structure>()
        {
            new Structure(0, null, null),

            new Structure(1, "Construction Yard",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Ghost Building/Blue Construction Yard (Ghost)")),

            new Structure(2, "Power Plant",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Ghost Building/Blue Power Plant (Ghost)")),

            new Structure(3, "Barracks",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Ghost Building/Blue Barracks (Ghost)")),

            new Structure(4, "War Factory",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Ghost Building/Blue War Factory (Ghost)"))

        };
    }

}
