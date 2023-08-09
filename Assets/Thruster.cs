using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public bool enabled;
    public float power;
    public float sensitivity = 1;
    ParticleSystem particles;
    public enum Binding {right, left, forward, boost };
    public Binding bound;
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
    }
    public void SetBinding(Binding binding)
    {
        Thruster thruster = gameObject.GetComponent<Thruster>();
        ShipMovementManager manager = FindObjectOfType<ShipMovementManager>();
        manager.driveGroup.Remove(thruster);
        manager.leftGroup.Remove(thruster);
        manager.rightGroup.Remove(thruster);
        if(binding == Binding.forward)
        {
            manager.driveGroup.Add(thruster);
            bound = binding;
        }
        else if(binding == Binding.right)
        {
            manager.rightGroup.Add(thruster);
            bound = binding;
        }
        else if(binding == Binding.left)
        {
            manager.leftGroup.Add(thruster);
            bound = binding;
        }
        /*int index = 0;
        foreach(Thruster thruster in manager.driveGroup)
        {
            
            if (thruster == gameObject.GetComponent<Thruster>())
            {
                manager.driveGroup.RemoveAt(index);
            }
            index++;
        }*/
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
