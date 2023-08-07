using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public bool enabled;
    public float power;
    public float sensitivity = 1;
    ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(particles.isPlaying && !enabled){
                particles.Stop();
                
        }
        if(enabled){
            if(!particles.isEmitting){
                print("play");
                particles.Stop();
                particles.Play();
            }
            gameObject.transform.parent.parent.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(-gameObject.transform.up*sensitivity*power,gameObject.transform.position);
        }
    }
}
