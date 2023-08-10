using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class StationAgent : Agent
{
    Rigidbody2D rb;
    Vector2 Target;
    public Thruster tn;
    public Thruster ts;
    public Thruster tnw;
    public Thruster tne;
    public Thruster tsw;
    public Thruster tse;
    // Start is called before the first frame update
    void Start()
    {
        Target = transform.localPosition;
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
        this.rb.angularVelocity = Random.Range(-10,10);
        this.rb.velocity = Random.insideUnitCircle*1;
        this.gameObject.transform.localPosition = Vector2.zero;

        Target = this.gameObject.transform.localPosition;
        // Move the target to a new spot
        //this.gameObject.transform.position = Vector2.zero;
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
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);
        sensor.AddObservation(rb.angularVelocity);
        sensor.AddObservation(tn.power);
        sensor.AddObservation(tne.power);
        sensor.AddObservation(tnw.power);
        sensor.AddObservation(ts.power);
        sensor.AddObservation(tse.power);
        sensor.AddObservation(tsw.power);
    }
    float time = 0;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        tn.power = actionBuffers.ContinuousActions[0];
        ts.power = -actionBuffers.ContinuousActions[1];
        tne.power = actionBuffers.ContinuousActions[2];
        tnw.power = actionBuffers.ContinuousActions[3];
        tse.power = actionBuffers.ContinuousActions[4];
        tsw.power = actionBuffers.ContinuousActions[5];
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target);
        time += Time.deltaTime;
        // Reached target
        AddReward((-distanceToTarget / 2 + 1) * Time.deltaTime);
        //print((-distanceToTarget / 2 + 1) * Time.deltaTime);
        // Fell off platform
        if (time > 10)
        {
            EndEpisode();
        }
        if (distanceToTarget > 10)
        {
            EndEpisode();
        }
    }
}
