using UnityEngine;
using System.Collections.Generic;
using System;
using System.Diagnostics;

public class StormCloudEvent : EventsClass {

    public float Speed;
    private GameObject stormPrefab;
    private GameObject tempStorm;
    private TurbineObject selectedTurbine;
    private Vector3 turbinePos;

    Stopwatch Timer;

    // Use this for initialization
    void Start () {
        Speed = 100.0f;
        Timer = Stopwatch.StartNew();
        stormPrefab = (GameObject)Resources.Load("StormCloud");
        EventStart();
    }
	
	// Update is called once per frame
	void Update () {
        
        UnityEngine.Debug.Log("Stopwatch: " + Timer.Elapsed );
        tempStorm.transform.position = Vector3.MoveTowards(tempStorm.transform.position, new Vector3(turbinePos.x, turbinePos.y + 60,turbinePos.z), Speed * Time.deltaTime);
    }

    public override void EventStart()
    {
        
        selectedTurbine = TurbineObject.all[EventHandler.rnd];
        turbinePos = selectedTurbine.transform.position;
        tempStorm = (GameObject)Instantiate(stormPrefab, turbinePos + new Vector3(600, 30, 0), Quaternion.identity);
        
    }

    public override void EventEnd()
    {
       
    }
   
}
