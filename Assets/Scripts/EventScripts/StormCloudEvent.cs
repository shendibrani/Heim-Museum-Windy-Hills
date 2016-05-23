using UnityEngine;
using System.Collections.Generic;
using System;
using System.Diagnostics;

public class StormCloudEvent : EventsClass {

    public float Speed;
    public GameObject stormCloud;
    private GameObject tempCloud;
    public int difficulty = 3;
   
    private Vector3 stormSpawnPos;

    public override void EventStart()
    {
        stormSpawnPos = new Vector3(UnityEngine.Random.Range(500.0f, 700.0f), 60.0f, UnityEngine.Random.Range(50.0f, 300.0f));
        tempCloud = (GameObject)Instantiate(stormCloud, stormSpawnPos, Quaternion.identity);
    }

    public override void EventEnd()
    {
       
    }
    
}
