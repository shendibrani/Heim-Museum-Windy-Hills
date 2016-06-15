using UnityEngine;
using System.Collections.Generic;

public class Boat : MonoBehaviour, IWindSensitive {

	//[SerializeField] Transform dest;
    public List<Transform> Destinations;
    public List<Transform> StartPositions;
    NavMeshAgent boatAgent;
    Transform StartNode;
    Transform EndNode;
	bool windAffected;
	bool isBroken = false;
	bool arrived;

	int step = 0;
	float stepTimer = 0;
	[SerializeField] float breakTime = 1;

	[SerializeField] float maxSpeed = 8f;
	[SerializeField] GameObject Sail1;
	[SerializeField] GameObject Sail2;
	[SerializeField] GameObject Sail3;

    void Start() {
        boatAgent = GetComponent<NavMeshAgent>();
        StartPositions.Add(GameObject.Find("StartPos1").transform);
        StartPositions.Add(GameObject.Find("StartPos2").transform);
        Destinations.Add(GameObject.Find("Destination1").transform);
        Destinations.Add(GameObject.Find("Destination2").transform);

        StartNode = StartPositions[Random.Range(0, StartPositions.Count)];
        EndNode = Destinations[Random.Range(0, StartPositions.Count)];

        boatAgent.Warp(StartNode.transform.position);
        transform.position = StartNode.transform.position;
        boatAgent.SetDestination(EndNode.transform.position);

    }

   
    void Update()
	{
        if (Vector3.Distance(transform.position, boatAgent.destination) < 2f)
		{
            arrived = true;
            Destroy(gameObject);
        }

		if (windAffected && !isBroken)
        {
			if (stepTimer >= breakTime)
			{
				if (step == 0)
				{
					step = 1;
				}
				else if (step == 1)
				{
					step = 2;
					//animation and color
				}
				else if (step == 2)
				{
					Break ();
					step = 0;
				}

				stepTimer = 0;
			}
			else
			{
				stepTimer += Time.deltaTime;
			}
			boatAgent.speed = Mathf.Lerp(boatAgent.speed, maxSpeed, 0.05f);
        }
		else if (!windAffected)
		{
		stepTimer = 0;
           boatAgent.speed = Mathf.Lerp(boatAgent.speed, 4f, 0.05f);
        }
    }

  
    public void OnExitWindzone()
	{
        windAffected = false;
    }

    public void OnStayWindzone()
    {
        windAffected = true;
       // Debug.Log("Boat: OnStayWindzone");
    }

    public void OnEnterWindzone()
    {
        //Debug.Log("Boat: OnEnterWindzone");

    }

	void Break()
	{
		isBroken = true;
		GameObject sailP = new GameObject ();
		sailP.transform.position = this.transform.position;
		sailP.transform.Rotate (0, 35, 0);

		Sail1.transform.parent = sailP.transform;
		Sail2.transform.parent = sailP.transform;
		Sail3.transform.parent = sailP.transform;


		sailP.AddComponent<Sail> ();

		RaycastHit hit;
		if (Physics.Raycast(this.transform.position + new Vector3 (0,5,0),sailP.transform.forward,out hit))
		{
			if (hit.collider.gameObject.GetComponent<TurbineObject>() != null)
			{
				sailP.GetComponent<Sail> ().goal = hit.collider.gameObject.GetComponent<TurbineObject> ();
			}
		}
		boatAgent.Stop ();
	}
}
