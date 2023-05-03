using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankController : MonoBehaviour
{
    public int testnumber = 0;
    public GameObject testobject;

    public int Team;

    [Header("Required Componants")]
    public Transform HitPoint;
    public SphereCollider rangeFinder;
    public GameObject BulletSpawn;
    public GameObject Projectile;


    [Header("Shooting/Attacking")]
    public int damage;
    public GameObject target;
    public bool CanShoot;
    public bool isChasing;
    public bool moveTowards;
    public float shootingInterval;

    [Header("Positional Values")]
    public Vector3 CurrentTile;
    public Vector3 Destination;
    public Vector3 LastPosition;

    private bool gotDestination;
    private NavMeshAgent agent;
    private Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<Grid>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.isStopped = true;
        gotDestination = false;
        if (transform.gameObject.tag == "RedUnit")
        {
            agent.destination = Destination;
            agent.isStopped = false;
            gotDestination = true;
        }

        if(testnumber == 5)
        {
            agent.destination = Destination;
            agent.isStopped = false;
            gotDestination = true;
        }
        InvokeRepeating("UpdateDictionary", 0.1f, 0.3f);
        InvokeRepeating("CheckDestination", 0.1f, 0.3f);
        
    }

    /// <summary>
    /// update will check if the unit needs to move towards a target. Also checks to see if a target has reached it's destination.
    /// last check is to see if the unit has a target. If not, it will stop shooting and chasing.
    /// </summary>
    void Update()
    {
        if (agent.isStopped == false)
        {
            if (moveTowards == true)
            {
                MoveTowardsTarget();
            }

            if ((transform.position.x == Destination.x) && (transform.position.z == Destination.z))
            {
                CancelInvoke();
                agent.isStopped = true;
                gotDestination = false;
                return;
            }
        }

        if ((CanShoot == true || isChasing == true || moveTowards == true) && (target == null))
        {
            CanShoot = false;
            target = null;
            isChasing = false;
            moveTowards = false;
            StopCoroutine(ShootProjectile(null, null));
            FindNewTarget();
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
        agent.isStopped = false;

        if (gotDestination == false)
        {
            InvokeRepeating("UpdateDictionary", 0.1f, 0.3f);
            InvokeRepeating("CheckDestination", 0.1f, 0.3f);
            gotDestination = true;
        }

    }

    public void UpdateDictionary()
    {
        CurrentTile = grid.GetNearestPointOnGrid(transform.position);
        grid.dictCoords[CurrentTile] = gameObject;

        if (LastPosition != null)
        {
            if (LastPosition != CurrentTile)
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
            if (Vector3.Distance(transform.position, Destination) < 1.0f)
            {
                if ((grid.dictCoords[Destination].GetComponent<TankController>()) && (grid.dictCoords[Destination] != this.gameObject))
                {
                    TankController tc = grid.dictCoords[Destination].GetComponent<TankController>();
                    //Debug.Log(this.gameObject.name + " is detecting " + grid.dictCoords[Destination] + " object in dict at " + Destination);
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
        //Debug.Log("unit bumped");
    }

    public void MoveTowardsTarget()
    {
        if (target.GetComponent<TankController>())
        {
            agent.SetDestination(grid.GetNearestPointOnGrid(target.transform.position - ((transform.position - target.transform.position).normalized * 3)));
            Destination = new Vector3(agent.destination.x, 0f, agent.destination.z);
            return;
        }

        if (target.GetComponent<StructureController>())
        {
            agent.SetDestination(grid.GetNearestPointOnGrid(target.transform.position - ((transform.position - target.transform.position).normalized * 3)));
            Destination = new Vector3(agent.destination.x, 0f, agent.destination.z);
            return;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if ((isChasing == true) && (other.gameObject == target))
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Unit"))
            {
                TankController tank = other.gameObject.GetComponent<TankController>();
                agent.destination = grid.GetNearestPointOnGrid(transform.position);
                Destination = new Vector3(agent.destination.x, 0, agent.destination.z);
                CanShoot = true;
                StartCoroutine(ShootProjectile(tank.HitPoint, BulletSpawn));
                moveTowards = false;
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Structure"))
            {
                StructureController structure = other.gameObject.GetComponent<StructureController>();
                agent.destination = grid.GetNearestPointOnGrid(transform.position);
                Destination = new Vector3(agent.destination.x, 0, agent.destination.z);
                CanShoot = true;
                StartCoroutine(ShootProjectile(structure.HitPoint, BulletSpawn));
                moveTowards = false;
                return;
            }
        }

        if (target == null)
        {
            if (other.gameObject.GetComponent<TankController>())
            {
                TankController tank = other.gameObject.GetComponent<TankController>();
                if (tank.Team != this.Team)
                {
                    target = other.gameObject;
                    CanShoot = true;
                    StartCoroutine(ShootProjectile(tank.HitPoint, BulletSpawn));
                    return;
                }
            }
            else if (other.gameObject.GetComponent<StructureController>())
            {
                StructureController structure = other.gameObject.GetComponent<StructureController>();
                if (structure.Team != this.Team)
                {
                    target = other.gameObject;
                    CanShoot = true;
                    StartCoroutine(ShootProjectile(structure.HitPoint, BulletSpawn));
                    return;
                }
            }
        }
    }

    /// <summary>
    /// when an anemy leaves the range of a unit, if chasing is true, they will follow and chase the unit down.
    /// if not, the unit is assumed to be defending, so they will jsut stop shooting at the target.
    /// </summary>

    private void OnTriggerExit(Collider other)
    {
        if (target != null)
        {

            if (isChasing == true && target.gameObject == other.gameObject)
            {
                CanShoot = false;
                moveTowards = true;
                StopCoroutine(ShootProjectile(null, null));
                return;
            }

            if (target.gameObject == other.gameObject)
            {
                target = null;
                CanShoot = false;
                StopCoroutine(ShootProjectile(null, null));
                return;
            }
        }
    }

    public IEnumerator ShootProjectile(Transform target, GameObject spawner)
    {
        while (CanShoot == true)
        {
            if (Vector3.Distance(transform.position, target.position) <= rangeFinder.radius)
            {
                GameObject projectileGO = Instantiate(Projectile);
                ProjectileController projectileController = projectileGO.GetComponent<ProjectileController>();
                projectileGO.transform.position = spawner.transform.position;
                Vector3 direction = (target.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                projectileGO.transform.rotation = targetRotation;
                projectileController.target = target;
                projectileController.Team = Team;
                projectileController.Damage = damage;
                yield return new WaitForSeconds(shootingInterval);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void FindNewTarget()
    {
        agent.destination = grid.GetNearestPointOnGrid(new Vector3(transform.position.x, 0, transform.position.z));
        isChasing = false;
        moveTowards = false;

        Collider[] colliders = Physics.OverlapSphere(transform.position, rangeFinder.radius);
        float closestDistance = float.MaxValue;
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<TankController>() && collider.gameObject.activeInHierarchy)
            {
                TankController tankController = collider.gameObject.GetComponent<TankController>();
                if (tankController.Team != Team)
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        target = tankController.gameObject;
                    }
                }
            }
        }
        if(target == null)
        {
            CanShoot = false;
        }
        else
        {
            CanShoot = true;
            StartCoroutine(ShootProjectile(target.GetComponent<TankController>().HitPoint, BulletSpawn));
        }
    }

        public void OnDestroy()
        {
            grid.dictCoords[CurrentTile] = null;
        }
    }

