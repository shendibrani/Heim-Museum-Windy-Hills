using UnityEngine;
using System.Collections.Generic;
using System;
using System.Diagnostics;

public class StormCloudEvent : EventsClass, IWindSensitive {

    public float Speed;
    //private GameObject stormPrefab;
    //private GameObject tempStorm;
    private TurbineObject selectedTurbine;
    private Vector3 turbinePos;
    private Vector3 stormSpawnPos;

    // Use this for initialization
    void Start () {
        
      //  stormPrefab = (GameObject)Resources.Load("StormCloud");
        EventStart();
    }
	
	// Update is called once per frame
	void Update () {
        if (TurbineObject.all.Count > 0) {
            
            MoveCloud();
            if (EnteredWindzone) transform.Translate(5, 0, 5);

        }
    }

    public override void EventStart()
    {
        transform.position = new Vector3(UnityEngine.Random.Range(500.0f, 700.0f), 60.0f, UnityEngine.Random.Range(50.0f, 300.0f));
       // tempStorm = (GameObject)Instantiate(stormPrefab, stormSpawnPos, Quaternion.identity);
    }

    void MoveCloud() {
        if (this != null)
            transform.Translate(-5, 0, 0);
    }

    public override void EventEnd()
    {
       
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "DestroyerOfClouds") {
            Destroy(this.gameObject);
        }

    }

    bool EnteredWindzone = false;
    public void OnEnterWindzone()
    {
        EnteredWindzone = true;
        
    }

    public void OnExitWindzone()
    {
        EnteredWindzone = false;
    }

    public void OnStayWindzone()
    {

    }
}
