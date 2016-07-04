using UnityEngine;
using System.Collections;

public class FireDepartment : Station 
{
	bool spawned;

	protected override void Start () 
	{
		base.Start ();
		Dispatcher<FiremenMessage>.Subscribe(SendFiremen);
	}

	void SendFiremen(FiremenMessage rm) 
	{
		GetFreeAgent().Send (rm.Sender.GetComponent<TurbineObject> ());
	}
}
