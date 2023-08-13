using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class StationAgent : Agent
{
    Rigidbody2D rb;
    //Vector2 Target;
    public SpriteRenderer sprite;
    public Thruster tn;
    public Thruster ts;
    public Thruster tnw;
    public Thruster tne;
    public Thruster tsw;
    public Thruster tse;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        oldist = 0;
        //Target = transform.localPosition;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target);

        // Reached target
        //AddReward((-distanceToTarget / 5 + 1) * Time.deltaTime);
        print("Distance is: " + distanceToTarget.ToString() + ", Reward is: " + ((-distanceToTarget / 2 + 1) * Time.deltaTime).ToString()) ;
        // Fell off platform
        if (distanceToTarget > 10)
        {
            EndEpisode();
        }*/
    }
    public override void OnEpisodeBegin()
    {
        time = 0;
        //this.rb.angularVelocity = Random.Range(-10,10);
        this.rb.velocity = Vector2.zero;
        this.gameObject.transform.localPosition = Vector2.zero;
        sprite.color = Random.ColorHSV(0,1,1,1,0.25f,0.75f);
        target.transform.localPosition = Random.insideUnitCircle*5;
        //Target = this.gameObject.transform.localPosition;
        // Move the target to a new spot
        this.gameObject.transform.localPosition = Vector2.zero;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        //sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition.x);
        sensor.AddObservation(this.transform.localPosition.y);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);
        sensor.AddObservation(target.localPosition.x);
        sensor.AddObservation(target.localPosition.y);
        sensor.AddObservation(rb.angularVelocity);
        sensor.AddObservation(this.transform.localRotation.eulerAngles.z);
        /*sensor.AddObservation(tn.power);
        sensor.AddObservation(tne.power);
        sensor.AddObservation(tnw.power);
        sensor.AddObservation(ts.power);
        sensor.AddObservation(tse.power);
        sensor.AddObservation(tsw.power);*/
    }
    float time = 0;
    public float modifier = 10;
    float oldist;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //rb.AddForce(new Vector2(actionBuffers.ContinuousActions[0]*modifier,modifier*actionBuffers.ContinuousActions[1]));
        //rb.AddTorque(actionBuffers.ContinuousActions[2]*modifier);
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.position);
        
        time += Time.deltaTime;
        // Reached target
        //AddReward((-distanceToTarget / 2) *2 * Time.deltaTime);
        /*tne.power = actionBuffers.ContinuousActions[0];
        tnw.power = -actionBuffers.ContinuousActions[0];
        tse.power = actionBuffers.ContinuousActions[0];
        tsw.power = -actionBuffers.ContinuousActions[0];
        tn.power = actionBuffers.ContinuousActions[1];
        ts.power = -actionBuffers.ContinuousActions[1];*/
        rb.AddForce(new Vector2(actionBuffers.ContinuousActions[0], actionBuffers.ContinuousActions[1])*modifier);
        AddReward(oldist-distanceToTarget*Time.deltaTime);
        //AddReward((-rb.velocity.magnitude) * Time.deltaTime);
        //AddReward((-this.transform.localPosition.magnitude+5)*Time.deltaTime);
        //print((-distanceToTarget / 2 + 1) * Time.deltaTime);
        // Fell off platform
        if (time > 25)
        {
            //AddReward(25);
            EndEpisode();
        }
        if (distanceToTarget > 10)
        {
            AddReward(-5);
            EndEpisode();
            
        }
        if (distanceToTarget < 0.5)
        {
            AddReward(Time.deltaTime);
            EndEpisode();
        }
        oldist = distanceToTarget;
    }
}
