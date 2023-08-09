using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ShipMovementManager : MonoBehaviour
{
    public InputActionAsset InputActions;
    public Vector2 movement;
    public GameObject muzzle;
    public GameObject bullet;
    public List<Thruster> driveGroup = new List<Thruster>();
    public List<Thruster> leftGroup = new List<Thruster>();
    public List<Thruster> rightGroup = new List<Thruster>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(Thruster thruster in driveGroup)
        {
            thruster.bound = Thruster.Binding.forward;
        }
        foreach (Thruster thruster in leftGroup)
        {
            thruster.bound = Thruster.Binding.left;
        }
        foreach (Thruster thruster in rightGroup)
        {
            thruster.bound = Thruster.Binding.right;
        }
        //print("start?");
        //InputActions.FindActionMap("Player").FindAction("Move").performed += Move;
        //InputActions.FindActionMap("Player").FindAction("Boost").performed += Boost;
    }
    public void Shoot(InputAction.CallbackContext context){
        Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation);
    }
    private void Boost(InputAction.CallbackContext context){
        //print("beep");
    }
    public void Stop(InputAction.CallbackContext context){
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity=0;
    }
    public void Move(InputAction.CallbackContext context){
        //print("move");
        //Debug.Log(context.ReadValue<Vector2>());
        movement = context.ReadValue<Vector2>();
        
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 up2D = new Vector2(transform.up.x, transform.up.y);
        foreach(Thruster thruster in driveGroup){
            if(movement.y > 0){
                thruster.power = movement.y;
                thruster.enabled = true;
            }
            else{
                thruster.enabled = false;
            }
        }
        foreach(Thruster thruster in leftGroup){
            if(movement.x > 0){
                thruster.power = movement.x;
                thruster.enabled = true;
            }
            else{
                thruster.enabled = false;
            }
        }
        foreach(Thruster thruster in rightGroup){
            if(movement.x < 0){
                thruster.power = -movement.x;
                thruster.enabled = true;
            }
            else{
                thruster.enabled = false;
            }
        }
        //gameObject.GetComponent<Rigidbody2D>().velocity += movement.y*Time.deltaTime*up2D;
        //gameObject.GetComponent<Rigidbody2D>().angularVelocity += -movement.x*20*Time.deltaTime;
    }
}
