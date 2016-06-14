using UnityEngine;
using System.Collections.Generic;

public class Boat : MonoBehaviour, IWindSensitive {

	//[SerializeField] Transform dest;
    public List<Transform> Destinations;
    public List<Transform> StartPositions;
    NavMeshAgent boatAgent;
    Transform StartNode;
    Transform EndNode;

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

    bool arrived;
    void Update() {
        if (Vector3.Distance(transform.position, boatAgent.destination) < 2f) {
            arrived = true;
            Destroy(gameObject);
        }

        if (windAffected)
        {
            boatAgent.speed *= 1.009f;
        }
        else {
           boatAgent.speed = Mathf.Lerp(boatAgent.speed, 4f, 0.05f);
        }
    }

    bool windAffected;
    public void OnExitWindzone() {
        windAffected = false;
    }

    public void OnStayWindzone()
    {
        windAffected = true;
        Debug.Log("Boat: OnStayWindzone");
    }

    public void OnEnterWindzone()
    {
        Debug.Log("Boat: OnEnterWindzone");

    }
}
