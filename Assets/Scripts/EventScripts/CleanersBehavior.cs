using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class CleanersBehavior : MonoBehaviour {
    Stopwatch timer;
    TurbineObject targetTurbine;
    CleanersDepartment station;
    NavMeshAgent agent;

	[SerializeField] bool debug;

    void Start()
    {
        timer = new Stopwatch();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 10.0f;
        station = FindObjectOfType<CleanersDepartment>();
        Dispatcher<CleanupMessage>.Subscribe(SendCleaner);
    }

    bool arrived = false;
    bool extinguished;
    void Update()
    {
        if (targetTurbine != null)
        {
            float distanceToTurbine = Vector3.Distance(targetTurbine.transform.position, transform.position);
            float distanceToStation = Vector3.Distance(transform.position, station.transform.position);
            if(debug) UnityEngine.Debug.Log("Distance to turbine: " + distanceToTurbine);

            if (distanceToTurbine < 8.0f && !arrived)
            {
                timer.Start();
                arrived = true;
                if(debug) UnityEngine.Debug.Log("Cleaner moved to the turbine");
            }

            if (timer.Elapsed.Seconds > 0.1f)
            {
                timer.Stop();
                timer.Reset();
                agent.SetDestination(station.transform.position);
                extinguished = true;
                targetTurbine.state.value.OnCleanup();
            }

            if (arrived && extinguished && distanceToStation < 5.0f) Destroy(gameObject);
        }
    }

    public void SendCleaner(CleanupMessage fm)
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
        Dispatcher<CleanupMessage>.Unsubscribe(SendCleaner);
    }
}
