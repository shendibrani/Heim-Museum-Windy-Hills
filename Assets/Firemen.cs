using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class Firemen : MonoBehaviour {

    Stopwatch timer;
    TurbineObject selectedTurbine;
    GameObject station;
    NavMeshAgent agent;

    // Use this for initialization
	void Start () {
        timer = new Stopwatch();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 10.0f;
        station = GameObject.Find("FireStation");
        Dispatcher<FiremenMessage>.Subscribe(SendFireman);
       
	}

   
    bool arrived = false;
    bool extinguished;
	// Update is called once per frame
	void Update ()
    {

        if (selectedTurbine != null)
        {
            float distanceToTurbine = Vector3.Distance(selectedTurbine.transform.position, transform.position);
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
                agent.SetDestination(GameObject.Find("FireStation").transform.position);
                extinguished = true;
            }

            if (arrived && extinguished && distanceToStation < 5.0f) Destroy(gameObject);

        }

    }

    public void SendFireman(FiremenMessage fm)
    {
        agent.SetDestination(fm.Sender.transform.position);
        selectedTurbine = fm.Sender.GetComponent<TurbineObject>();
    }
    
    void OnDestroy() {
        Dispatcher<FiremenMessage>.Unsubscribe(SendFireman);
    }
}
