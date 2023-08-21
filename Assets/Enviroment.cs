using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    public float randomizationTime = 5;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >=randomizationTime){
            randomize();
            time=0;
        }
    }
    void randomize(){
        foreach(GameObject child in GameObject.FindGameObjectsWithTag("Obstacle")){
            print("kill");
            Destroy(child.gameObject);
        }
        GetComponent<SpawnObjects>().count = Random.Range(100,250);
        GetComponent<SpawnObjects>().Spawn();
    }
}
