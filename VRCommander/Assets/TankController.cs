using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankController : MonoBehaviour
{
    public int Team;
    public Vector3 Destination;
    public Vector3 LastPosition;

    private NavMeshAgent agent;
    private Grid grid;
    
    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<Grid>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.destination = Destination;
        InvokeRepeating("UpdateDictionary", 0.1f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isStopped == true)
        {
            CancelInvoke();
        }
    }

    public void SetDestination(Vector3 destination)
    {
        Destination = destination;
        agent.destination = Destination;
    }

    public void UpdateDictionary()
    {
        Vector3 currentTile = grid.GetNearestPointOnGrid(transform.position);
        grid.dictCoords[currentTile] = gameObject;

        if(LastPosition != null)
        {
            if(LastPosition != currentTile)
            {
                grid.dictCoords[LastPosition] = null;
            }
        }

        LastPosition = currentTile;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<TankController>())
        {
            TankController tank = other.gameObject.GetComponent<TankController>();
            if(tank.Team != this.Team)
            {
                Debug.Log("This unit is on team " + tank.Team);
                return;
            }
        }
        else if (other.gameObject.GetComponent<StructureController>())
        {
            StructureController structure = other.gameObject.GetComponent<StructureController>();
            if (structure.Team != this.Team)
            {
                Debug.Log("This structure is on team " + structure.Team);
            }
        }
    }
}
