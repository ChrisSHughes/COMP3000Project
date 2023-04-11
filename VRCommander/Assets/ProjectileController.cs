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
    public int Team;

    private Rigidbody rb;

    public float velocicty;


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

        this.transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            Debug.Log("detected unit collision");
            TankController tankController = collision.gameObject.GetComponent<TankController>();
            UnitHealthController healthController = collision.gameObject.GetComponent<UnitHealthController>();
            if (Team != tankController.Team)
            {
                Debug.Log("Different team, taking damage");
                healthController.TakeDamage(Damage);
                ExplodShell();
            }
            else
            {
                Debug.Log("Same Team, Ignoring Collisions");
                Physics.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider>(), gameObject.GetComponent<CapsuleCollider>());
            }
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Structure"))
        {

        }
        else
        {
            ExplodShell();
        }

    }

    private void ExplodShell()
    {
        // do animation for splosion

        Destroy(this.gameObject);
    }

}
