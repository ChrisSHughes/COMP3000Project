using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureController : MonoBehaviour
{
    public Vector3 origin;
    public int Team;
    public float sizex;
    public float sizez;

    public int MaxHealth = 0;
    public int Health = 0;

    public int TechLevelRequired;
    public int TechLevel;

    public bool Repairing = false;
    public bool RallyPointEnabled;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
