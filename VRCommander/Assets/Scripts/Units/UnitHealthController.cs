using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UnitHealthController : MonoBehaviour
{
    [Header("Health Components")]
    public float MaxHealth;
    public float CurrentHealth;

    [Header("Required Components")]
    public Canvas UICanvas;
    public Image HealthBackground;
    public Image HealthForeground;

    [Header("Gradient Components")]
    public Gradient ForegroundGradient;
    public Gradient BackgroundGradient;


    // Start is called before the first frame update, gives initial values to stuff
    void Start()
    {
        CurrentHealth = MaxHealth;
        HealthForeground.fillAmount = CurrentHealth / MaxHealth;
    }

    /// <summary>
    /// A function for calculating damage input and checking if a unit has died
    /// </summary>
    public void TakeDamage(GameObject projectile , int damage)
    {
        CurrentHealth -= damage;
        UpdateUI();
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// a function for calculating healing done to a unit
    /// </summary>

    public void HealDamage(int amount)
    {
        CurrentHealth += amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        HealthForeground.fillAmount = CurrentHealth / MaxHealth;
        HealthForeground.color = ForegroundGradient.Evaluate(HealthForeground.fillAmount);
        HealthBackground.color = BackgroundGradient.Evaluate(HealthForeground.fillAmount);
    }

    public void Die()
    {
        Destroy(this.gameObject);
        // here we can do death animation stuff.
    }
}
