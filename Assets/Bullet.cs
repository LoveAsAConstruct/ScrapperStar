using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed=10;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(transform.up.x, transform.up.y)*speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
