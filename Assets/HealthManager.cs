using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.EventSystems;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public UnityEvent deathEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Die(){
        deathEvent.Invoke();
    }
}
