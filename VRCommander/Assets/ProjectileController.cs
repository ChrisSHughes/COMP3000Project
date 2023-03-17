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


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 direction = (target.position - transform.position).normalized;
        rb.AddForce(direction * InitialForce, ForceMode.Impulse);
    }

    private void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        rb.AddForce(direction * Speed, ForceMode.Force);

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, RotationSpeed * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Team == collision.gameObject.GetComponent<TankController>().Team)
        {
            Debug.Log("Ignoreing collision between " + collision.gameObject.name + " and " + gameObject.name);
            Physics.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider>(), gameObject.GetComponent<CapsuleCollider>());
        }
    }

}
