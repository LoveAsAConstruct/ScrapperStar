using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countermeasure : MonoBehaviour
{
    public float life;
    float time = 0;
    Vector3 scale;
    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.localScale = Vector3.Lerp(scale,Vector3.zero, time/life);
        if(time>=life){
            Destroy(gameObject);
        }
    }
}
