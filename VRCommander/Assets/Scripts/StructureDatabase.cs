using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureDatabase : MonoBehaviour
{

    public List<Structure> redStructures = new List<Structure>();
    public List<Structure> blueStructures = new List<Structure>();

    public List<Structure> ghostRedStructures = new List<Structure>();
    public List<Structure> ghostBlueStructures = new List<Structure>();


    public void BuildRedStructureList()
    {
        redStructures = new List<Structure>()
        {
            new Structure(0, null, null),

            new Structure(1, "Construction Yard",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Solid Building/Red Construction Yard")),

            new Structure(2, "Power Plant",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Solid Building/Red PowerPlant")),

            new Structure(3, "Barracks",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Solid Building/Red Barracks")),

            new Structure(4, "War Factory",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Solid Building/Red War Factory"))

        };
    }

    public void BuildBlueStructureList()
    {
        blueStructures = new List<Structure>()
        {
            new Structure(0, null, null),

            new Structure(1, "Construction Yard",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Solid Building/Blue Construction Yard")),

            new Structure(2, "Power Plant",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Solid Building/Blue PowerPlant")),

            new Structure(3, "Barracks",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Solid Building/Blue Barracks")),

            new Structure(4, "War Factory",Resources.Load<GameObject>("Prefabs/GameObjects/Buildings/Solid Building/Blue War Factory"))

        };
    }

    public void BuildRedGhostStructureList()
    {
        ghostRedStructures = new List<Structure>()
        {


        };
    }

    public void BuildBlueGhostStructureList()
    {
        ghostBlueStructures = new List<Structure>()
        {


        };
    }

}
