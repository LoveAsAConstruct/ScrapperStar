using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public int count;
    public float range;
    public GameObject asteroid;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < count; i++){
            Instantiate(asteroid, Random.insideUnitCircle*range, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
