using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealthController : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    /// <summary>
    /// A function for calculating damage input and checking if a unit has died
    /// </summary>
    public void TakeDamage(int damage)
    {
        Debug.Log("Taking damage");
        CurrentHealth -= damage;
        if(CurrentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// a function for calculating healing done to a unit
    /// </summary>
    public void HealDamage()
    {

    }

    public void Die()
    {
        // here we can do death animation stuff.
    }
}
