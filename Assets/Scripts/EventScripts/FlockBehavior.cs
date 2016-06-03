using UnityEngine;
using System.Collections;
using System.Diagnostics;


public class FlockBehavior : MonoBehaviour
{

    
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
        if (distance < DamageRadius && effectApplied == false) {
            selectedTurbined.BreakTurbine();
            effectApplied = true;
            timer.Start();
            transform.LookAt(selectedTurbined.transform);
        }
    }

    bool moveToWindmill = true;

    void MoveToWindMill()
    {
        if (moveToWindmill && !effectApplied) { 
            
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(selectedTurbined.transform.position.x, selectedTurbined.transform.position.y + 30, selectedTurbined.transform.position.z), 1.5f);
            transform.LookAt(selectedTurbined.transform);
        }
        if (timer.Elapsed.Seconds > 3) moveToWindmill = false;
        MoveAway();
    }

    void MoveAway() {
        if (!moveToWindmill && effectApplied) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - 300, transform.position.y, transform.position.z), 1.5f);
            if (transform.position.x < -20) Destroy(gameObject);
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
