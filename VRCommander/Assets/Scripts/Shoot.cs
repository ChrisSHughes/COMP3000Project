using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    public float velocity = 50f;
    public GameObject bulletObj;
    public Transform frontOfGun;

    public AudioSource gunCock;
    public AudioClip gunShot;
    public AudioClip pickup;

    public void Start()
    {
        gunCock = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        gunCock.PlayOneShot(gunShot);
        GameObject pistolBullet = Instantiate(bulletObj, frontOfGun.position, frontOfGun.rotation);
        pistolBullet.GetComponent<Rigidbody>().velocity = velocity * frontOfGun.forward;
        Destroy(pistolBullet, 5f);
    }

    public void PickupSound()
    {
        gunCock.PlayOneShot(pickup);
    }
}
