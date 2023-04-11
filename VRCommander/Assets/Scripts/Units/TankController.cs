using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankController : MonoBehaviour
{
    public int Team;
    public Transform HitPoint;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isStopped == true)
        {
            CancelInvoke();
            gotDestination = false;
        }
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
        }
        gotDestination = true;
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
                ShootProjectile(tank.HitPoint, BulletSpawn);
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

    public void OnCollisionEnter(Collision collision)
    {

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
