using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    [Header("HEALTH PARAMETERS")]
    [SerializeField] private string[] damageTags;
    [SerializeField] private float health;
    [SerializeField] private float healthMax;
    
    [Header("DECAY PARAMETERS")]
    [SerializeField] private bool doesDecay = false;
    [SerializeField] private float decayTickRate;
    [SerializeField] private float decayDamage;
    
    [Header("DEATH PARAMETERS")]
    [SerializeField] private bool destroyOnDeath;
    [SerializeField] private float destroyOnDeathDelay;
    
    private Animator anim;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        health = healthMax;
    }
    
    #region BostonCity Campus
    /* This code is property of Boston City Campus, and should not be used, or copied for any other subject.
     If you would copy and or use this in an assessment outside the assessment that this was supplied for
     action will be taken against you for plaigirism.
    */
    #endregion
    
    // Update is called once per frame
    void Update()
    {
        if (doesDecay)
        {
            StartCoroutine(DecayHealth());
        }
        
        if (health <= 0)
        {
            DeathHandler();
        }
    }

    private IEnumerator DecayHealth()
    {
        yield  return new WaitForSeconds(decayTickRate);
        DamageHandler(decayDamage);
    }

    public void DamageHandler(string damageTag, float damageAmount)
    {
        if (damageTags.Contains(damageTag))
        {
            if (damageAmount <= health)
            {
                health -= damageAmount;
            }
            else
            {
                health = 0;
            }
            
        }
    }

    public void DamageHandler(float damageAmount)
    {
        if (damageAmount <= health)
        {
            health -= damageAmount;
        }
        else
        {
            health = 0;
        }
    }

    public void HealHandler(float healAmount)
    {
        if (health + healAmount >= healthMax)
        {
            health = healthMax;
        }
        else
        {
            health += healAmount;
        }
    }

    public void DeathHandler()
    {
        if (destroyOnDeath)
        {
            anim.SetTrigger("Death"); //Animation Call
            Destroy(gameObject, destroyOnDeathDelay);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SetHealth(float health)
    {
        this.health = health;
    }

    public void SetMaxHealth(float healthMax)
    {
        this.healthMax = healthMax;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return healthMax;
    }

}
