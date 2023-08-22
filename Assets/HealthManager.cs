using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
public class HealthManager : MonoBehaviour
{
    public UnityEvent deathEvent;
    public UnityEvent damageEvent;
    public float maxHealth;
    public float health = 0;
    public Slider healthSlider;
    public bool destroyOnDeath = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(healthSlider != null)
        {
            float val = health / maxHealth;
            healthSlider.value = Mathf.Lerp(0, healthSlider.maxValue, val);
        }
    }
    public void Damage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            deathEvent.Invoke();
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
        }
        print("damage");
        damageEvent.Invoke();
    }
    public void Die(){
        deathEvent.Invoke();
    }
}
