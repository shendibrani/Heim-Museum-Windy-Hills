using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class FlockBehavior : MonoBehaviour
{
    float DamageRadius = 15.0f;
    public float Speed;
    Stopwatch timer;
    
    int randomed;
    TurbineObject selectedTurbine;
    public bool debug = true;
    
    // Update is called once per frame
    void Start()
    {
        timer = new Stopwatch();
        if (TurbineObject.all.Count > 0)
        {
            randomed = Random.Range(0, TurbineObject.all.Count);
            selectedTurbine = TurbineObject.all[randomed];
        }
    }

    void Update()
    {
        if (TurbineObject.all.Count > 0)
        {
            MoveToWindMill();
            WindEffect();    
        }

        MoveAway();
    }

    bool effectApplied = false;
    void WindEffect()
    {
        float distance = Vector3.Distance(new Vector3(selectedTurbine.transform.position.x, 30, transform.position.z), this.transform.position);
        if (distance < DamageRadius && effectApplied == false) {
            TurbineStateManager.dirtyState.Copy(selectedTurbine);
            effectApplied = true;
            timer.Start();
         
        }
    }

    bool moveToWindmill = true;

    void MoveToWindMill()
    {
        if (moveToWindmill && !effectApplied) { 
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(selectedTurbine.transform.position.x, selectedTurbine.transform.position.y + 30, selectedTurbine.transform.position.z), Speed);
            transform.LookAt(selectedTurbine.transform);
        }
        if (timer.Elapsed.Seconds > 3)
            moveToWindmill = false;
    }

    void MoveAway() {
        if (!moveToWindmill && effectApplied) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - 300, transform.position.y + 50, transform.position.z), Speed);
            transform.LookAt(new Vector3(transform.position.x - 300, transform.position.y, transform.position.z));
            if (transform.position.x < -20) Destroy(gameObject);
        }
    }

    public void SetTargetTurbine(TurbineObject to)
    {
        selectedTurbine = to;
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
