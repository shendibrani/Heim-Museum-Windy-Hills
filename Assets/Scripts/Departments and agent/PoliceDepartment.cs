using UnityEngine;
using System.Collections;

public class PoliceDepartment : Station 
{
	bool spawned;

	protected override void Start () 
	{
		base.Start ();
		Dispatcher<PoliceMessage>.Subscribe(SendPolicemen);
	}

	void SendPolicemen(PoliceMessage rm) 
	{
		GetFreeAgent().Send (rm.Sender.GetComponent<TurbineObject> ());
	}
}
