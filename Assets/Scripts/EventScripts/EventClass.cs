using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class EventClass 
{   
	public int difficulty;

    abstract public void EventStart();
    abstract public void EventEnd();

	protected TurbineObject GetRandomTurbine()
	{
		return TurbineObject.all[RNG.Next(0,TurbineObject.all.Count)];
	}
}

[System.Serializable]
public class StormCloudEvent : EventClass {

	[SerializeField] float speed;

	public StormCloudEvent(){}

	public override void EventStart()
	{
		Vector3 stormSpawnPos = new Vector3(UnityEngine.Random.Range(600.0f, 700.0f), 60.0f, UnityEngine.Random.Range(50.0f, 300.0f));
		GameObject instance = (GameObject) GameObject.Instantiate(Resources.Load("StormCloud"), stormSpawnPos, Quaternion.identity);
		instance.GetComponent<StormBehavior>().speed = speed;
	}

	public override void EventEnd() {}
}

[System.Serializable]
public class FireEvent : EventClass
{
	public override void EventStart ()
	{
		TurbineStateManager.lowFireState.Copy(GetRandomTurbine());
	}

	public override void EventEnd ()
	{}
}

[System.Serializable]
public class SaboteurEvent : EventClass
{
	public override void EventStart ()
	{
		TurbineStateManager.saboteurState.Copy(GetRandomTurbine());
	}

	public override void EventEnd ()
	{}
}

[System.Serializable]
public class FlockEvent : EventClass
{
	public override void EventStart ()
	{
        Vector3 stormSpawnPos = new Vector3(UnityEngine.Random.Range(500.0f, 700.0f), 60.0f, UnityEngine.Random.Range(50.0f, 300.0f));
        GameObject instance = (GameObject)GameObject.Instantiate(Resources.Load("PigeonEventMovement"), stormSpawnPos, Quaternion.identity);
       
    }

	public override void EventEnd ()
	{

    }
}
