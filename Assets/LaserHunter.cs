using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class LaserHunter : Agent
{
    public bool training;
    public ParticleSystem laser;
    Rigidbody2D rb;
    public Color wincolor;
    public float range;
    public float damage;
    public float power;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public override void OnEpisodeBegin()
    {
        if(training){
            this.range = 5;
            time = 0;
            //this.rb.angularVelocity = Random.Range(-10,10);
            this.rb.velocity = Vector2.zero;
            this.gameObject.transform.localPosition = Random.insideUnitCircle*15;
            this.rb.angularVelocity = 0;
            this.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0,360));
            this.power = Random.Range(6,25);
        }
        
        //sprite.color = Random.ColorHSV(0, 1, 1, 1, 0.25f, 0.75f);
        //target.transform.localPosition = Random.insideUnitCircle * 5;
        //Target = this.gameObject.transform.localPosition;
        // Move the target to a new spot
        //this.gameObject.transform.localPosition = Vector2.zero;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
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
        
        //sensor.AddObservation(this.transform.localPosition.x);
        //sensor.AddObservation(this.transform.localPosition.y);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);
        sensor.AddObservation(rb.angularVelocity);
        sensor.AddObservation(this.transform.forward.x);
        sensor.AddObservation(this.transform.forward.y);
        //sensor.AddObservation(rb.angularVelocity);
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
        AddReward(-5);
        EndEpisode();

    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        time += Time.deltaTime;
        
        rb.AddForce(new Vector2(gameObject.transform.up.x, gameObject.transform.up.y) * (Mathf.Abs((actions.ContinuousActions[0])) * power));
        rb.AddTorque((actions.ContinuousActions[1])*0.25f/6*power);
        AddReward(-Mathf.Abs(actions.ContinuousActions[1]*Time.deltaTime)*4f);
        AddReward(Time.deltaTime);
        
        RaycastHit2D hit;
        hit = Physics2D.Raycast(gameObject.transform.position, new Vector2(transform.up.x, transform.up.y),range);
        //Debug.DrawLine(gameObject.transform.position, new Vector3(hit.point.x,hit.point.y,0),Color.yellow, Time.deltaTime*10);
        
        if(hit.collider != null){
            if(hit.collider.gameObject.GetComponent<LaserEnemyAgent>() != null){
                if(actions.ContinuousActions[2]!=1){
                    AddReward(-Time.deltaTime*10);
                }
            }
        }
        if(actions.ContinuousActions[2]>0.25f){
            
            Debug.DrawRay(transform.position, new Vector2(transform.up.x, transform.up.y)*range, Color.red,Time.deltaTime*range);
            if(hit.collider != null){
                if(hit.collider.gameObject.GetComponent<HealthManager>() != null){
                    AddReward(50);
                    if (training)
                    {
                        hit.collider.gameObject.GetComponent<HealthManager>().Die();
                        if (hit.collider.gameObject.tag == "HostileTarget")
                        {
                            AddReward(25);
                        }
                        if (hit.collider.gameObject.tag == "Obstacle")
                        {
                            AddReward(-60);
                        }
                        StopAllCoroutines();
                        StartCoroutine(WinFade(0.1f));
                    }
                    else
                    {
                        hit.collider.gameObject.GetComponent<HealthManager>().Damage(damage*Time.deltaTime);
                        print("damage");
                    }
                    
                    //EndEpisode();
                }
                else{
                    AddReward(-0.5f*Time.deltaTime);
                }
            
            }
            else{
                AddReward(-0.5f*Time.deltaTime);
            }
        }
        else{
            Debug.DrawRay(transform.position, new Vector2(transform.up.x, transform.up.y)*range, Color.blue,Time.deltaTime*range);
        }            
        if(Vector3.Distance(transform.position, Vector3.zero)>75){
            AddReward(-50);
            EndEpisode();
        }
        if(Vector3.Distance(transform.position, Vector3.zero)>59){
            AddReward(-5*Time.deltaTime);
        }
        if(actions.ContinuousActions[2]>0.25f){
            if(!laser.isPlaying){
                laser.Play();
            }
        }
        else{
            laser.Stop();
        }
        /*FT.power = (actions.ContinuousActions[0] + 1) / 2;
        RT.power = actions.ContinuousActions[1];
        a
        LT.power = -actions.ContinuousActions[1];
        SRT.power = -actions.ContinuousActions[1];
        SLT.power = actions.ContinuousActions[1];*/
        //AddReward(-distanceToTarget*2 * Time.deltaTime);
        //AddReward(-Mathf.Abs(rb.angularVelocity) / 90 * Time.deltaTime);
        //AddReward((-rb.velocity.magnitude) * Time.deltaTime);
        //AddReward((-this.transform.localPosition.magnitude+5)*Time.deltaTime);
        //print((-distanceToTarget / 2 + 1) * Time.deltaTime);
        // Fell off platform
        /*if (time > 25)
        {
            //AddReward(-1);
            EndEpisode();
        }*/
        /*if (distanceToTarget > 10)
        {
            AddReward(-5);
            EndEpisode();

        }*/
        /*if (distanceToTarget < 0.5)
        {
            AddReward(Time.deltaTime*(25-time));
            //EndEpisode();
        }*/

        base.OnActionReceived(actions);
    }
    IEnumerator WinFade(float time){
        print("win");
        //time = time;
        Camera.main.backgroundColor = wincolor;
        for(float t = 0; t<time; t+= Time.deltaTime/Time.timeScale){
            Camera.main.backgroundColor = Color.Lerp(wincolor, Color.black, t/time);
            yield return new WaitForEndOfFrame();
        }
        Camera.main.backgroundColor = Color.black;
        yield break;
    }
    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D other) {
            AddReward(-100);
            EndEpisode();
        }
    void Update()
    {
    
    }
}
