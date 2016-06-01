using UnityEngine;
using System.Collections;
using System.Diagnostics;


public class FlockBehavior : MonoBehaviour
{

    float WindRadius = 60.0f;
    float DamageRadius = 27.0f;

    Stopwatch timer;
    
    int randomed;
    TurbineObject selectedTurbined;
    public bool debug = true;
    // Update is called once per frame
    void Start()
    {
        timer = new Stopwatch();
        if (TurbineObject.all.Count > 0)
        {
            randomed = Random.Range(0, TurbineObject.all.Count);
            selectedTurbined = TurbineObject.all[randomed];
        }
    }

    void Update()
    {
        if (TurbineObject.all.Count > 0)
        {
            MoveToWindMill();
            WindEffect();
            
           
        }
    }

    bool effectApplied = false;
    void WindEffect()
    {
        float distance = Vector3.Distance(new Vector3(selectedTurbined.transform.position.x, 30, transform.position.z), this.transform.position);
        if (distance <= DamageRadius & effectApplied == false) {
            selectedTurbined.BreakTurbine();
            effectApplied = true;
            timer.Start();
            
        }

    }
    bool moveToWindmill = true;

    void MoveToWindMill()
    {
        if (moveToWindmill) { 
            Transform target = TurbineObject.all[randomed].transform;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y + 30, target.position.z), 1.5f);
            transform.LookAt(target);
        }
        if (timer.Elapsed.Seconds > 3) moveToWindmill = false;
        MoveAway();
    }

    void MoveAway() {
        if (!moveToWindmill) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - 300, transform.position.y, transform.position.z), 1.5f);
            if (transform.position.x < -20) Destroy(gameObject);
        }

    }

    void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, WindRadius);
            Gizmos.DrawSphere(transform.position, DamageRadius);
        }
    }

    public void OnEnterWindzone()
    {


    }

    public void OnExitWindzone()
    {

    }

    public void OnStayWindzone()
    {

    }
}
