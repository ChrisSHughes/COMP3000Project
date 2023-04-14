using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankController : MonoBehaviour
{
    public int Team;
    public Transform HitPoint;

    public Vector3 CurrentTile;
    public Vector3 Destination;
    public Vector3 LastPosition;
    public GameObject BulletSpawn;
    public GameObject Projectile;

    private bool gotDestination;
    private NavMeshAgent agent;
    private Grid grid;
    
    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<Grid>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.destination = Destination;
        gotDestination = true;
        InvokeRepeating("UpdateDictionary", 0.1f, 0.3f);
        InvokeRepeating("CheckDestination", 0.1f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isStopped == false)
        {
            if ((transform.position.x == Destination.x) && (transform.position.z == Destination.z))
            {
                Debug.Log("Stopped");
                CancelInvoke();
                agent.isStopped = true;
                gotDestination = false;
                return;
            }
        }

        //if (agent.isStopped == true)
        //{
        //    Debug.Log("Stopped");
        //    CancelInvoke();
        //    gotDestination = false;
        //}
    }

    /// <summary>
    /// Set destincation is used to.. set the destination of the unit and have the unit move via the AI componant. There is a check to see if the unit is already moving, so we 
    /// do not accidentally invoke the dictionaryUpdate method more than one time at any one time. As this will cause performance issues.
    /// </summary>
    public void SetDestination(Vector3 destination)
    {
        Destination = destination;
        agent.destination = Destination;

        if(gotDestination == false)
        {
            InvokeRepeating("UpdateDictionary", 0.1f, 0.3f);
            InvokeRepeating("CheckDestination", 0.1f, 0.3f);
        }
        gotDestination = true;
    }

    public void UpdateDictionary()
    {
        CurrentTile = grid.GetNearestPointOnGrid(transform.position);
        grid.dictCoords[CurrentTile] = gameObject;

        if(LastPosition != null)
        {
            if(LastPosition != CurrentTile)
            {
                grid.dictCoords[LastPosition] = null;
            }
        }
        LastPosition = CurrentTile;
    }

    public void CheckDestination()
    {
        if (grid.dictCoords[Destination] == null)
        {
            return;
        }
        else
        {
            if (grid.dictCoords[Destination].GetComponent<TankController>())
            {
                if (Vector3.Distance(transform.position, Destination) < 1.0f)
                {
                    TankController tc = grid.dictCoords[Destination].GetComponent<TankController>();
                    tc.BumpUnit();
                }
            }
            else if (grid.dictCoords[Destination].GetComponent<StructureController>())
            {
                //make function to have unit find new space.
            }
        }
    }

    public void BumpUnit()
    {
        Debug.Log("unit bumped");
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<TankController>())
        {
            TankController tank = other.gameObject.GetComponent<TankController>();
            if(tank.Team != this.Team)
            {
                ShootProjectile(tank.HitPoint, BulletSpawn);
                return;
            }
        }
        else if (other.gameObject.GetComponent<StructureController>())
        {
            StructureController structure = other.gameObject.GetComponent<StructureController>();
            if (structure.Team != this.Team)
            {
                ShootProjectile(structure.transform, BulletSpawn);
                return;
            }
        }
    }

    public void ShootProjectile(Transform target, GameObject spawner)
    {
        GameObject projectile = Instantiate(Projectile);
        ProjectileController pc = projectile.GetComponent<ProjectileController>();
        projectile.transform.position = spawner.transform.position;
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        projectile.transform.rotation = targetRotation;
        pc.target = target;
        pc.Team = Team;
    }
}
