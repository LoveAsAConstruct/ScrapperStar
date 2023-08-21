using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
//using Unity.Mathematics;

public class HideEnemy : Agent
{
    //public bool training;
    ////public GameObject target;
    //public Rigidbody2D targetrb;
    //public ParticleSystem laser;
    Rigidbody2D rb;
    public GameObject countermeasure;
    public float power;
    // Start is called before the first frame update
    void Start()
    {
        //print("helloooo?");
        rb = GetComponent<Rigidbody2D>();
       // print("anyonethere?");
    }
    public override void OnEpisodeBegin()
    {
        //print("episodeBegin");
        if(true){
            //this.range = Random.Range(1,20);
            time = 0;
            //this.rb.angularVelocity = Random.Range(-10,10);
            this.rb.velocity = Vector2.zero;
            this.gameObject.transform.localPosition = Random.insideUnitCircle*15;
            this.rb.angularVelocity = 0;
            this.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0,360));
            this.power = Random.Range(6,15);
        }
        
        //sprite.color = Random.ColorHSV(0, 1, 1, 1, 0.25f, 0.75f);
        //target.transform.localPosition = Random.insideUnitCircle * 5;
        //Target = this.gameObject.transform.localPosition;
        // Move the target to a new spot
        //this.gameObject.transform.localPosition = Vector2.zero;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //print("heuristic");
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        if(Input.GetKey(KeyCode.Space)){
            continuousActionsOut[2] = 1;
        }
        else{
            continuousActionsOut[2] = 0;
        }
        
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);
        sensor.AddObservation(rb.angularVelocity);
        sensor.AddObservation(this.transform.forward.x);
        sensor.AddObservation(this.transform.forward.y);
        base.CollectObservations(sensor);
        //print("lased");
        //RaycastHit2D hit;
        //hit = Physics2D.Raycast(gameObject.transform.position+gameObject.transform.up, new Vector2(transform.up.x, transform.up.y));
        /*if(hit.point != null){
            print(hit.point);
            Debug.DrawLine(gameObject.transform.position, hit.point,Color.green, Time.deltaTime*10);
            
        }
        Debug.DrawRay(transform.position, new Vector2(transform.up.x, transform.up.y)*1, Color.red,Time.deltaTime*10);*/
        //Debug.DrawLine(gameObject.transform.position, hit.point);
    }
    float time = 0;
    public void Hit(){
        AddReward(-75);
        EndEpisode();

    }
    bool chaffed = false;
    public override void OnActionReceived(ActionBuffers actions)
    {
        //print("actions.ContinuousActions[0]");
        rb.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * (Mathf.Abs((actions.ContinuousActions[0])) * power));
        rb.AddTorque((actions.ContinuousActions[1])*0.25f/6*power);
        AddReward(-Mathf.Abs(actions.ContinuousActions[1]*Time.deltaTime)*1f);
        AddReward(Time.deltaTime*3);
        if(actions.ContinuousActions[2] > 0.75f){
            Instantiate(countermeasure, gameObject.transform.position, Quaternion.identity);
            AddReward(-5f*Time.deltaTime);
            chaffed = true;
        }
        if(Vector3.Distance(transform.position, Vector3.zero)>75){
            AddReward(-50);
            EndEpisode();
        }
        if(Vector3.Distance(transform.position, Vector3.zero)>59){
            AddReward(-5*Time.deltaTime);
        }
        if(actions.ContinuousActions[2] <= 0.75f){
            chaffed = false;
        }
        base.OnActionReceived(actions);
    }
    
    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D other) {
        AddReward(-100);
        EndEpisode();
    }
    void Update()
    {
        //print("i am up[dating)']");
    }
}
