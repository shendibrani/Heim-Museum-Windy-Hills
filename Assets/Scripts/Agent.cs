using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class Agent : MonoBehaviour {

	Stopwatch timer;
	protected TurbineObject targetTurbine;
	public GameObject station;
	public float stopTime = 1f;
	[SerializeField] NavMeshAgent agent;

	[SerializeField] public UIState type;

	[SerializeField] bool debug;

	void Start()
	{
		timer = new Stopwatch();
		agent.speed = 10.0f;
	}

	public bool busy { get { return targetTurbine != null; }}

	void Update()
	{
		if (targetTurbine != null) 
		{
			UnityEngine.Debug.Log ("Car leaving");
			float distanceToTurbine = Vector3.Distance (targetTurbine.transform.position, transform.position);
			if (debug)
				UnityEngine.Debug.Log ("Distance to turbine: " + distanceToTurbine);

			if (distanceToTurbine < 8.0f) {

				timer.Start ();
				switch (type) {
				case UIState.Police:
					targetTurbine.state.value.OnPolice ();
					break;
				case UIState.Firemen:
					targetTurbine.state.value.OnFiremen ();
					break;
				case UIState.Cleanup:
					targetTurbine.state.value.OnCleanup ();
					break;
				case UIState.Repair:
					targetTurbine.state.value.OnRepair ();
					break;
				}

				targetTurbine = null;
				if (debug)
					UnityEngine.Debug.Log ("Fireman moved to the turbine");
			}
				
		} else {
			if (timer.Elapsed.Seconds > stopTime) {
				UnityEngine.Debug.Log ("Car returning");
				timer.Stop ();
				timer.Reset ();
				agent.SetDestination (station.transform.position);

			}
		}
	}

	public void Send(TurbineObject target)
	{
		if (!busy)
		{
			targetTurbine = target;
			agent.SetDestination(target.transform.position);
		}
	}
}
