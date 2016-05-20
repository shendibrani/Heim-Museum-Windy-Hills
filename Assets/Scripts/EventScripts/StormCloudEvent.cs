using UnityEngine;
using System.Collections.Generic;
using System;
using System.Diagnostics;

public class StormCloudEvent : EventsClass, IWindSensitive {

    public float Speed;
    private GameObject stormPrefab;
    private GameObject tempStorm;
    private TurbineObject selectedTurbine;
    private Vector3 turbinePos;
    private Vector3 stormTargetPosition;

    Stopwatch Timer;
  

    // Use this for initialization
    void Start () {
        Speed = 100.0f;
        stormPrefab = (GameObject)Resources.Load("StormCloud");
        EventStart();
    }
	
	// Update is called once per frame
	void Update () {
        
        
        MoveCloudToTurbine();
        startStormFunctions();
    }

    public override void EventStart()
    {

        selectedTurbine = TurbineObject.all[EventHandler.rnd];
        turbinePos = selectedTurbine.transform.position;
        stormTargetPosition = new Vector3(turbinePos.x, turbinePos.y + 30, turbinePos.z);
        tempStorm = (GameObject)Instantiate(stormPrefab, turbinePos + new Vector3(600, 30, 0), Quaternion.identity);

    }

    void MoveCloudToTurbine() {
        if (tempStorm != null)
        tempStorm.transform.position = Vector3.MoveTowards(tempStorm.transform.position, stormTargetPosition , Speed * Time.deltaTime);
    }

    void DestroyAfterTime(int seconds) {
        if (Timer.Elapsed.Seconds > seconds)
        {
            Timer.Stop();
            UnityEngine.Debug.Log("Destroyed StormCloud");
            Destroy(tempStorm);
        }
        
    }

    public void OnEnterWindzone()
    {
        isInWindzone = true;
    }

    public void OnExitWindzone()
    {
        isInWindzone = false;
    }

    bool StormPassedScreen() {
       
            if (tempStorm != null && tempStorm.transform.position == stormTargetPosition)
            {
                UnityEngine.Debug.Log("storm is above turbine!");
                return true;
           }
        else return false;
    }

    bool timerStart = false;
    private void startStormFunctions() {
        if (StormPassedScreen())
        {
            if (!timerStart)
            {
                Timer = Stopwatch.StartNew();
                timerStart = true;
            }

            UnityEngine.Debug.Log("Stopwatch: " + Timer.Elapsed.Seconds + " second(s)");
            DestroyAfterTime(10);
        }
    }

    public override void EventEnd()
    {
       
    }
   
}
