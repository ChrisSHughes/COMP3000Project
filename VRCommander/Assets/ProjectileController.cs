using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    public int Damage;
    public float Speed;
    public float InitialForce;
    public float RotationSpeed;
    public Transform target;
    public GameObject Shooter;
    public int Team;
    public float velocicty;

    private Rigidbody rb;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 direction = (target.position - transform.position).normalized;
        rb.AddForce(direction * InitialForce, ForceMode.Impulse);
    }

    private void Update()
    {
        velocicty = rb.velocity.magnitude;
        Vector3 direction = (target.position - transform.position).normalized;
        rb.AddForce(direction * Speed, ForceMode.Acceleration);

        transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            TankController tankController = collision.gameObject.GetComponent<TankController>();
            UnitHealthController tankHealthController = collision.gameObject.GetComponent<UnitHealthController>();
            if (Team != tankController.Team)
            {
                tankHealthController.TakeDamage(gameObject, Damage);
                ExplodShell();
                return;
            }
            else
            {
                Physics.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider>(), gameObject.GetComponent<CapsuleCollider>());
            }
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Structure"))
        {
            StructureController structureController = collision.gameObject.GetComponent<StructureController>();
            StructureHealthController structureHealthController = collision.gameObject.GetComponent<StructureHealthController>();
            if (Team != structureController.Team)
            {
                structureHealthController.TakeDamage(gameObject, Damage);
                ExplodShell();
            }
            else
            {
                Physics.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider>(), gameObject.GetComponent<CapsuleCollider>());
            }
        }
        else
        {
            ExplodShell();
        }

    }

    private void ExplodShell()
    {
        // do animation for splosion
        Debug.Log("destroying shell");
        Destroy(this.gameObject);
    }

    public void ResetTarget()
    {
        Debug.Log("Resetting Target");
        Shooter.GetComponent<TankController>().CanShoot = false;
        Shooter.GetComponent<TankController>().target = null;
    }

}
