﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[System.Serializable]
public abstract class EventClass 
{   
	public int difficulty;
    public EventNames name;
    public TurbineObject usedTurbine;

    abstract public void EventStart();
    abstract public void FakeEventStart();
    abstract public void EventEnd();

	protected TurbineObject GetRandomTurbine()
	{
		return TurbineObject.all[RNG.Next(0, TurbineObject.all.Count)];
	}
}

public enum EventNames
{
     Boat, StormCloud, Fire, Saboteur, Flock
}

[System.Serializable]
public class BoatEvent : EventClass 
{
	public List<Transform> StartPositions;
	Transform StartNode;

    public EventNames Name
    {
        get
        {
            return name;
        }
    }

    public BoatEvent() 
	{
        name = EventNames.Boat;
    }
    
    public override void EventStart() 
	{
		StartPositions.Add(GameObject.Find("StartPos1").transform);
		StartPositions.Add(GameObject.Find("StartPos2").transform);
		StartNode = StartPositions[UnityEngine.Random.Range(0, StartPositions.Count)];

		GameObject instance = (GameObject)GameObject.Instantiate(Resources.Load("Boat"), StartNode.position, Quaternion.identity);
    }

    public override void FakeEventStart() { EventStart(); }
    public override void EventEnd() {}
}

[System.Serializable]
public class StormCloudEvent : EventClass {

	[SerializeField] float speed;

    public EventNames Name
    {
        get
        {
            return name;
        }
    }

    public StormCloudEvent(){
        name = EventNames.StormCloud;
    }

	public override void EventStart()
	{
		Vector3 stormSpawnPos = new Vector3(UnityEngine.Random.Range(400, 460), 60, UnityEngine.Random.Range(60, 200));
		GameObject instance = (GameObject) GameObject.Instantiate(Resources.Load("StormCloud"), stormSpawnPos, Quaternion.identity);
		instance.GetComponent<StormBehavior>().speed = speed;
	}
    public override void FakeEventStart() { EventStart(); }
    public override void EventEnd() {}
}

[System.Serializable]
public class FireEvent : EventClass
{
    public EventNames Name
    {
        get
        {
            return name;
        }
    }

    public FireEvent(){
        name = EventNames.Fire;
    }

    public override void EventStart ()
	{
		TurbineStateManager.lowFireState.Copy(GetRandomTurbine());
   	}

    public override void FakeEventStart() { EventStart(); }
    public override void EventEnd () {}
}

[System.Serializable]
public class SaboteurEvent : EventClass
{
  
    public EventNames Name
    {
        get
        {
            return name;
        }
    }

    public SaboteurEvent()
    {
        name = EventNames.Saboteur;
    }

    public override void EventStart ()
	{
		TurbineStateManager.saboteurState.Copy (GetRandomTurbine());
    }

    public override void FakeEventStart() 
	{
        EventStart();
    }
    public override void EventEnd ()
	{}
}

[System.Serializable]
public class FlockEvent : EventClass
{

    public EventNames Name
    {
        get
        {
            return name;
        }
    }
    public FlockEvent()
    {
        name = EventNames.Flock;
    }

    public override void EventStart ()
	{
        Vector3 stormSpawnPos = new Vector3(UnityEngine.Random.Range(500.0f, 700.0f), 60.0f, UnityEngine.Random.Range(50.0f, 300.0f));
        GameObject instance = (GameObject)GameObject.Instantiate(Resources.Load("PigeonEventMovement"), stormSpawnPos, Quaternion.identity);
    }
    public override void FakeEventStart() { EventStart(); }
    public override void EventEnd () {}
}
