using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure
{

    public int id;
    public string name;
    public GameObject building;

    public Structure(int id, string name, GameObject building)
    {
        this.id = id;
        this.name = name;
        this.building = building;

    }
}
