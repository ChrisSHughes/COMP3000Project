using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    public float velocity = 50f;
    public GameObject bulletObj;
    public Transform frontOfGun;

    public void Fire()
    {
        GameObject pistolBullet = Instantiate(bulletObj, frontOfGun.position, frontOfGun.rotation);
        pistolBullet.GetComponent<Rigidbody>().velocity = velocity * frontOfGun.forward;
        Destroy(pistolBullet, 5f);
    }
}
