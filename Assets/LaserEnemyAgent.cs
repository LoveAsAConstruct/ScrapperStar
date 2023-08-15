using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class LaserEnemyAgent : Agent
{
    public GameObject target;
    Rigidbody2D rb;
    public Thruster FT;
    public Thruster LT;
    public Thruster RT;
    public Thruster SLT;
    public Thruster SRT;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public override void OnEpisodeBegin()
    {
        time = 0;
        //this.rb.angularVelocity = Random.Range(-10,10);
        this.rb.velocity = Vector2.zero;
        this.gameObject.transform.localPosition = Vector2.zero;
        this.rb.angularVelocity = 0;
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        //sprite.color = Random.ColorHSV(0, 1, 1, 1, 0.25f, 0.75f);
        target.transform.localPosition = Random.insideUnitCircle * 5;
        //Target = this.gameObject.transform.localPosition;
        // Move the target to a new spot
        this.gameObject.transform.localPosition = Vector2.zero;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[1] = Input.GetAxis("Horizontal");
        continuousActionsOut[0] = Input.GetAxis("Vertical");
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition.x);
        sensor.AddObservation(this.transform.localPosition.y);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);
        sensor.AddObservation(target.transform.localPosition.x);
        sensor.AddObservation(target.transform.localPosition.y);
        sensor.AddObservation(rb.angularVelocity);
        sensor.AddObservation(this.transform.rotation.eulerAngles.z);
        //sensor.AddObservation(rb.angularVelocity);
        base.CollectObservations(sensor);
    }
    float time = 0;
    public override void OnActionReceived(ActionBuffers actions)
    {
        time += Time.deltaTime;
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.transform.position);
        rb.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * (Mathf.Abs((actions.ContinuousActions[0])) * 6));
        rb.AddTorque(actions.ContinuousActions[1]*0.25f);
        /*FT.power = (actions.ContinuousActions[0] + 1) / 2;
        RT.power = actions.ContinuousActions[1];
        LT.power = -actions.ContinuousActions[1];
        SRT.power = -actions.ContinuousActions[1];
        SLT.power = actions.ContinuousActions[1];*/
        AddReward(-distanceToTarget*2 * Time.deltaTime);
        AddReward(-Mathf.Abs(rb.angularVelocity) / 90 * Time.deltaTime);
        //AddReward((-rb.velocity.magnitude) * Time.deltaTime);
        //AddReward((-this.transform.localPosition.magnitude+5)*Time.deltaTime);
        //print((-distanceToTarget / 2 + 1) * Time.deltaTime);
        // Fell off platform
        if (time > 25)
        {
            AddReward(10-distanceToTarget);
            EndEpisode();
        }
        /*if (distanceToTarget > 10)
        {
            AddReward(-5);
            EndEpisode();

        }*/
        if (distanceToTarget < 0.5)
        {
            AddReward(Time.deltaTime*(25-time));
            //EndEpisode();
        }

        base.OnActionReceived(actions);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
