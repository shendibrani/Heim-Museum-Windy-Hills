using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class FiremenBehavior : MonoBehaviour {

    Stopwatch timer;
    TurbineObject targetTurbine;
    FireDepartment station;
    NavMeshAgent agent;

    //Use this for initialization
	void Start () {
        timer = new Stopwatch();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 10.0f;
        station = FindObjectOfType<FireDepartment>();
        Dispatcher<FiremenMessage>.Subscribe(SendFireman);
    }
    
    bool arrived = false;
    bool extinguished;
    void Update ()
    {
        if (targetTurbine != null)
        {
            float distanceToTurbine = Vector3.Distance(targetTurbine.transform.position, transform.position);
            float distanceToStation = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(station.transform.position.x, station.transform.position.z));
            UnityEngine.Debug.Log("Distance to turbine: " + distanceToTurbine);

            if (distanceToTurbine < 8.0f && !arrived)
            {
                timer.Start();
                arrived = true;
                UnityEngine.Debug.Log("Fireman moved to the turbine");
            }

            if (timer.Elapsed.Seconds > 2.0f)
            {
                timer.Stop();
                timer.Reset();
                agent.SetDestination(station.transform.position);
                extinguished = true;
                targetTurbine.state.value.OnFiremen();
            }

            if (arrived && extinguished && distanceToStation < 10.0f) Destroy(gameObject);
        }
    }

    public void SendFireman(FiremenMessage fm)
    {
        if (fm.Sender.GetComponent<TurbineObject>() == targetTurbine && !extinguished)
        {
            agent.SetDestination(targetTurbine.transform.position);
        }
       
    }

    public void SetTargetTurbine(TurbineObject to)
    {
        targetTurbine = to;
    }
    
    void OnDestroy()
    {
        Dispatcher<FiremenMessage>.Unsubscribe(SendFireman);
    }
}
