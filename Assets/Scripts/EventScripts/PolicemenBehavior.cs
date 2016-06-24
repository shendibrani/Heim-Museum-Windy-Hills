using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class PolicemenBehavior : MonoBehaviour {

    Stopwatch timer;
    TurbineObject targetTurbine;
    PoliceDepartment station;
    NavMeshAgent agent;

    void Start()
    {
        timer = new Stopwatch();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 10.0f;
        station = FindObjectOfType<PoliceDepartment>();
        Dispatcher<PoliceMessage>.Subscribe(SendPoliceman);
    }

    bool arrived = false;
    bool extinguished;
    void Update()
    {
        if (targetTurbine != null)
        {
            float distanceToTurbine = Vector3.Distance(targetTurbine.transform.position, transform.position);
            float distanceToStation = Vector3.Distance(transform.position, station.transform.position);
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
                targetTurbine.state.value.OnPolice();
            }

            if (arrived && extinguished && distanceToStation < 5.0f) Destroy(gameObject);
        }
    }

    public void SendPoliceman(PoliceMessage fm)
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
        Dispatcher<PoliceMessage>.Unsubscribe(SendPoliceman);
    }
}
